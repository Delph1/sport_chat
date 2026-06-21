<script>
    import { toggleReaction } from '$lib/services/api';

    // Svelte 5: Så här tar vi emot data (props) från föräldern
    let { post } = $props();

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
                
                // Eftersom post är ett objekt skickat som prop, uppdateras
                // gränssnittet direkt när vi ändrar dess värden!
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
</script>

<article class="bg-white p-4 rounded-xl shadow-sm border border-gray-200 transition-all hover:shadow-md">
    <div class="flex justify-between items-baseline mb-2 border-b border-gray-100 pb-2">
        <span class="font-bold text-slate-900">{post.author || 'Anonym Supporter'}</span>
        <span class="text-xs text-gray-500 font-medium">Nyligen</span>
    </div>
    
    <p class="text-gray-800 leading-relaxed whitespace-pre-wrap mb-4">{post.content}</p>
    
    <div class="flex items-center pt-2 border-t border-gray-50">
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
    </div>
</article>