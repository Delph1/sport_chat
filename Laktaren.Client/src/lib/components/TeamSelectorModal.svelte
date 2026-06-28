<script>
    import { onMount } from 'svelte';
    import { getTeams, saveUserPreferences } from '$lib/services/api';

    let { isOpen = $bindable(), onSave } = $props();

    let availableTeams = $state([]);
    let selectedTeamId = $state(""); // Favoritlaget
    let secondaryTeams = $state([]); // Listan med följda lag
    let useTeamColors = $state(false);

    onMount(async () => {
        const response = await getTeams();
        if (response.ok) {
            availableTeams = await response.json();
        }
    });

    // Hanterar toggling av sekundära lag
    function toggleSecondaryTeam(teamId) {
        if (secondaryTeams.includes(teamId)) {
            secondaryTeams = secondaryTeams.filter(id => id !== teamId);
        } else {
            secondaryTeams = [...secondaryTeams, teamId];
        }
    }

    async function handleSave() {
        const response = await saveUserPreferences({
            teamId: selectedTeamId,
            secondaryTeams: secondaryTeams,
            useTeamColors: useTeamColors
        });

        if (response.ok) {
            isOpen = false;
            onSave();
        }
    }
</script>

{#if isOpen}
    <div class="fixed inset-0 bg-black/50 flex items-center justify-center z-50">
        <div class="bg-white p-6 rounded-xl w-96 shadow-xl">
            <h2 class="text-xl font-bold mb-4">Välj ditt lag</h2>
            
            <select bind:value={selectedTeamId} class="w-full p-2 border rounded mb-4">
                <option value="">Välj lag...</option>
                {#each availableTeams as team (team.id)}
                    <option value={team.id}>{team.name}</option>
                {/each}
            </select>

            <label class="flex items-center space-x-2 mb-6">
                <input type="checkbox" bind:checked={useTeamColors} />
                <span>Använd lagets färger</span>
            </label>
            <h3 class="text-lg font-bold mt-4">Följ även dessa lag:</h3>
            <div class="grid grid-cols-2 gap-2 mt-2">
                {#each availableTeams as team (team.id)}
                    <label class="flex items-center space-x-2">
                        <input 
                            type="checkbox" 
                            value={team.id} 
                            checked={secondaryTeams.includes(team.id)}
                            onchange={() => toggleSecondaryTeam(team.id)}
                        />
                        <span>{team.name}</span>
                    </label>
                {/each}
            </div>
            <button 
                onclick={handleSave}
                class="w-full bg-slate-900 text-white py-2 rounded-lg font-bold"
            >
                Spara profil
            </button>
        </div>
    </div>
{/if}