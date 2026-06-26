<script>
    import { getPosts, createPost } from '$lib/services/api';
    import { onMount } from 'svelte';
	import Post from '$lib/components/Post.svelte';

    // Svelte 5 Runes för reaktivitet
    let posts = $state([]);
    let isLoading = $state(true);
    let newPostContent = $state('');
    let errorMessage = $state('');
    let isPosting = $state(false);
    let isLoggedIn = $state(false);

    onMount(async () => {
        isLoggedIn = !!localStorage.getItem('token');
        
        try {
            const response = await getPosts();
            if (response.ok) {
                const response = await getPosts();
                if (response.ok) {
                    const data = await response.json();
                    console.log("Datan vi fick:", data);
                    posts = data.reverse(); 
                } else {
                    console.error("Backend sa nej...");
                }
            }
        } catch (error) {
            console.error("Kunde inte hämta flödet:", error);
        }
        finally {
            isLoading = false;
        }
    });

    async function handlePost(event) {
        event.preventDefault();
        if (!newPostContent.trim()) return;

        isPosting = true;
        errorMessage = '';

        try {
            // Vi skickar inlägget till backend
            const response = await createPost({ 
                content: newPostContent,
                parentPostId: null
            });

            if (response.ok) {
                const createdPost = await response.json();
                
                posts = [createdPost, ...posts]; 
                
                newPostContent = ''; 
            } else if (response.status === 401) {
                errorMessage = 'Din biljett har gått ut. Logga in igen!';
                handleLogout();
            } else {
                errorMessage = 'Domaren blåste av anropet. Försök igen.';
            }
        } catch (error) {
            errorMessage = 'Tekniskt fel i kommunikationen med arenan.';
        } finally {
            isPosting = false;
        }
    }
    
    function handleLogout() {
        localStorage.removeItem('token');
        localStorage.removeItem('userId');
        isLoggedIn = false;
    }
</script>

<div class="max-w-2xl mx-auto bg-gray-100 min-h-screen">
    <header class="bg-slate-900 text-white p-4 sticky top-0 z-10 shadow-md flex justify-between items-center">
        <h1 class="text-2xl font-black tracking-wider uppercase">Läktaren</h1>
        
        {#if isLoggedIn}
            <button onclick={handleLogout} class="text-sm font-bold text-gray-300 hover:text-white transition-colors">
                Lämna arenan
            </button>
        {:else}
            <a href="/auth" class="text-sm font-bold text-white hover:underline">
                Logga in
            </a>
        {/if}
    </header>

    <main class="p-4 space-y-6">
        {#if isLoggedIn}
            <section class="bg-white p-4 rounded-xl shadow-sm border border-gray-200">
                <form onsubmit={handlePost}>
                    <textarea 
                        bind:value={newPostContent}
                        class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900 focus:bg-white resize-none" 
                        rows="3" 
                        placeholder="Vad skriker du från läktaren just nu?"
                        required></textarea>
                    
                    {#if errorMessage}
                        <p class="text-red-600 text-sm mt-2 font-medium">{errorMessage}</p>
                    {/if}

                    <div class="flex justify-end mt-3">
                        <button type="submit" disabled={isPosting || !newPostContent.trim()} 
                            class="bg-slate-900 text-white px-6 py-2 rounded-lg font-bold hover:bg-slate-800 transition-colors disabled:opacity-50">
                            {isPosting ? 'Vrålar...' : 'Vråla'}
                        </button>
                    </div>
                </form>
            </section>
        {:else}
            <section class="bg-slate-200 p-6 rounded-xl text-center border border-slate-300">
                <h2 class="font-bold text-slate-800 mb-2">Du står utanför vändkorsen</h2>
                <p class="text-slate-600 mb-4 text-sm">Du kan läsa vad klacken skriker, men för att delta behöver du logga in.</p>
                <a href="/auth" class="inline-block bg-slate-900 text-white px-6 py-2 rounded-lg font-bold hover:bg-slate-800 transition-colors">
                    Hämta ut din biljett här
                </a>
            </section>
        {/if}

        <hr class="border-gray-300" />

        <section class="space-y-4">
            {#if isLoading}
                <p>Hämtar senaste vrålen från läktaren...</p>
            {:else}
                {#each posts as post, i (post.id)}
                    <Post bind:post={posts[i]} />
                {:else}
                    <p>Inga inlägg hittades på läktaren ännu. Var den första att vråla!</p>
                {/each}
            {/if}
        </section>
    </main>
</div>