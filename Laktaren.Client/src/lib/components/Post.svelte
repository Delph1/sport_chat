<script>
    import { toggleReaction, createPost, loadReplies } from '$lib/services/api';
    import Post from './Post.svelte';

    let { post = $bindable() } = $props();
    let showReplies = $state(false);
    let replies = $state([]);



    async function handleReplies() {
        if (showReplies) {
            showReplies = false;
            return;
        }
        try{
            const response = await loadReplies(post.id);
            if (response.ok) {
                const data = await response.json();
                replies = data;
                showReplies = true;
            }
        }
        catch (error) {
            console.error(error);
        }
    }
  
    async function handleReaction() {
        // Enkel koll om vi saknar biljett i webbläsaren
        if (!localStorage.getItem('token')) {
            alert("Du måste stå på läktaren (vara inloggad) för att kunna jubla!");
            return;
        }

        try {
            const response = await toggleReaction(post.id);
            
            if (response.ok) {
                const data = await response.json();
                
                if (data.liked) {
                    post.likeCount = (post.likeCount || 0) + 1;
                    post.userHasLiked = true; 
                } else {
                    post.likeCount = Math.max(0, (post.likeCount || 1) - 1);
                    post.userHasLiked = false;
                }
            } else if (response.status === 401) {
                alert("Din biljett har gått ut, logga in igen via menyn.");
            }
        } catch (error) {
            console.error("Det gick inte att nå domaren:", error);
        }
    }

    let showReplyForm = $state(false); 
    let replyContent = $state('');
    let isSubmitting = $state(false);

    async function handleReply() {
        if (!replyContent.trim()) return;
        if (!localStorage.getItem('token')) {
            alert("Du måste stå på läktaren (vara inloggad) för att svara!");
            return;
        }

        isSubmitting = true;
        try {
            const newPost = {
                content: replyContent,
                parentPostId: post.id 
            };

            // Vi skickar hela Post-objektet till api.ts
            const response = await createPost(newPost);
            
            if (response.ok) {
                replyContent = '';
                showReplyForm = false;
                
                post.replyCount = (post.replyCount || 0) + 1;
            } else {
                alert("Det gick inte att skicka svaret. Domaren kan ha blåst av spelet.");
            }
        } catch (error) {
            console.error(error);
        } finally {
            isSubmitting = false;
        }
    }
</script>

<article class="bg-white p-4 rounded-xl shadow-sm border border-gray-200 transition-all hover:shadow-md">
    <div class="flex justify-between items-baseline mb-2 border-b border-gray-100 pb-2">
        <span class="font-bold text-slate-900">{post.author || 'Anonym Supporter'}</span>
        <span class="text-xs text-gray-500 font-medium">Nyligen</span>
    </div>
    
    {#if post.isDeleted}
        <div class="p-4 bg-gray-50 border border-gray-200 rounded-xl italic text-gray-500">
            [Användaren ångrade sitt inlägg]
        </div>
    {:else}
        <p class="text-gray-800">{post.content}</p>
    {/if}
    
    <div class="flex items-center pt-2 border-t border-gray-50 space-x-6">
        <button 
            onclick={handleReaction}
            class="flex items-center space-x-1 group transition-colors {post.userHasLiked ? 'text-red-600' : 'text-gray-500 hover:text-red-500'}"
        >
            <svg xmlns="http://www.w3.org/2000/svg" 
                 class="h-5 w-5 transition-transform group-hover:scale-110 {post.userHasLiked ? 'fill-current' : 'fill-none'}" 
                 viewBox="0 0 24 24" 
                 stroke="currentColor" 
                 stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M4.318 6.318a4.5 4.5 0 000 6.364L12 20.364l7.682-7.682a4.5 4.5 0 00-6.364-6.364L12 7.636l-1.318-1.318a4.5 4.5 0 00-6.364 0z" />
            </svg>
            <span class="font-medium text-sm">
                {post.likeCount || 0}
            </span>
        </button>

        <button 
            onclick={() => showReplyForm = !showReplyForm} 
            class="flex items-center space-x-1 text-gray-500 hover:text-blue-500 transition-colors group"
        >
            <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5 transition-transform group-hover:scale-110" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M8 12h.01M12 12h.01M16 12h.01M21 12c0 4.418-4.03 8-9 8a9.863 9.863 0 01-4.255-.949L3 20l1.395-3.72C3.512 15.042 3 13.574 3 12c0-4.418 4.03-8 9-8s9 3.582 9 8z" />
            </svg>
            <span class="font-medium text-sm">{post.replyCount || 0}</span>
        </button>
        <button onclick={loadReplies} class="text-sm text-blue-600">
            {showReplies ? 'Dölj svar' : `Visa ${post.replyCount} svar`}
        </button>

        {#if showReplies}
            <div class="ml-8 border-l-2 border-gray-200 pl-4 mt-2">
                {#each replies as reply, i (reply.id)}
                    <Post bind:post={replies[i]} />
                {/each}
            </div>
        {/if}
    </div>

    {#if showReplyForm}
        <div class="mt-4 pt-3 border-t border-gray-100 flex flex-col space-y-2 animate-in fade-in slide-in-from-top-2 duration-200">
            <textarea 
                bind:value={replyContent}
                placeholder="Skriv din kontring..." 
                class="w-full text-sm border border-gray-300 rounded-lg p-2 focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none"
                rows="2"
            ></textarea>
            <div class="flex justify-end">
                <button 
                    onclick={handleReply}
                    disabled={isSubmitting || !replyContent.trim()}
                    class="bg-slate-900 text-white text-xs font-bold px-4 py-2 rounded-lg hover:bg-slate-800 disabled:opacity-50 transition-colors"
                >
                    {isSubmitting ? 'Skickar...' : 'Svara'}
                </button>
            </div>
        </div>
    {/if}

</article>