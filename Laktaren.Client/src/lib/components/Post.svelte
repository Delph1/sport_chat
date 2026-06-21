<script>
    // Svelte 5 sätt att ta emot props
    let { post } = $props();

    // Reaktivt framräknade värden med $derived
    let likesCount = $derived((post.reactions || []).filter(r => r.isLike).length);
    
    let dislikesByTeam = $derived((post.reactions || [])
        .filter(r => !r.isLike)
        .reduce((acc, reaction) => {
            // Fallback om lag saknas av någon anledning
            const teamName = reaction.team?.name || 'Okänt lag'; 
            acc[teamName] = (acc[teamName] || 0) + 1;
            return acc;
        }, {})
    );
</script>

<article class="bg-white p-4 rounded-xl shadow-sm border border-gray-200 transition-all hover:shadow-md">
    <div class="flex justify-between items-baseline mb-2 border-b border-gray-100 pb-2">
        <span class="font-bold text-slate-900">{post.author || 'Anonym Supporter'}</span>
        <span class="text-xs text-gray-500 font-medium">Nyligen</span>
    </div>
    
    <p class="text-gray-800 leading-relaxed whitespace-pre-wrap">{post.content}</p>

    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center pt-4 mt-3 border-t border-gray-100">
        
        <button class="flex items-center space-x-2 text-gray-500 hover:text-slate-900 transition-colors">
            <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 10h4.764a2 2 0 011.789 2.894l-3.5 7A2 2 0 0115.263 21h-4.017c-.163 0-.326-.02-.485-.06L7 20m7-10V5a2 2 0 00-2-2h-.095c-.5 0-.905.405-.905.905 0 .714-.211 1.412-.608 2.006L7 11v9m7-10h-2M7 20H5a2 2 0 01-2-2v-6a2 2 0 012-2h2.514"></path>
            </svg>
            <span class="font-bold">{likesCount}</span>
        </button>

        <div class="mt-3 sm:mt-0 text-sm">
            {#if Object.keys(dislikesByTeam).length > 0}
                <div class="flex flex-wrap gap-2 items-center">
                    <span class="text-gray-500 mr-1 text-xs uppercase tracking-wider font-bold">Rival-koll:</span>
                    {#each Object.entries(dislikesByTeam) as [teamName, count]}
                        <span class="bg-gray-100 px-2 py-1 rounded text-gray-700 text-xs font-medium">
                            {teamName} <strong class="text-red-500 ml-1">{count}</strong>
                        </span>
                    {/each}
                </div>
            {:else}
                <span class="text-gray-400 text-xs italic">Inga rivaler har reagerat än.</span>
            {/if}
        </div>
    </div>
</article>