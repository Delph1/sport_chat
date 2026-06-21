<script>

    import { registerUser, loginUser } from '$lib/services/api';

    let activeTab = $state('login'); 

    // Variabler för registrering
    let regUsername = $state('');
    let regEmail = $state('');
    let regPassword = $state('');

    // Variabler för inloggning
    let loginEmail = $state('');
    let loginPassword = $state('');
    
    // Status
    let errorMessage = $state('');
    let successMessage = $state('');
    let isLoading = $state(false);

    async function handleRegister(event) {
        event.preventDefault(); 
        errorMessage = '';
        successMessage = '';
        isLoading = true;

        try {
            const response = await registerUser({ 
                username: regUsername, 
                email: regEmail, 
                password: regPassword 
            });

            if (response.ok) {
                successMessage = 'Konto skapat! Byt flik för att logga in.';
                regUsername = ''; regEmail = ''; regPassword = '';
                setTimeout(() => activeTab = 'login', 2000);
            } else {
                errorMessage = 'Kunde inte skapa kontot. Kanske är e-posten redan registrerad?';
            }
        } catch (error) {
            errorMessage = 'Det gick inte att nå domaren (serverfel).';
        } finally {
            isLoading = false;
        }
    }

    async function handleLogin(event) {
        event.preventDefault();
        errorMessage = '';
        successMessage = '';
        isLoading = true;

        try {
            // Samma sak här, otroligt rent och snyggt
            const response = await loginUser({ 
                email: loginEmail, 
                password: loginPassword 
            });

            if (response.ok) {
                const data = await response.json();
                successMessage = 'Välkommen in på läktaren!';
                
                localStorage.setItem('token', data.token);
                localStorage.setItem('userId', data.userId);

                setTimeout(() => window.location.href = '/', 1000);
            } else {
                errorMessage = 'Felaktig e-post eller lösenord. Domaren dömer bort försöket.';
            }
        } catch (error) {
            errorMessage = 'Det gick inte att nå domaren (serverfel).';
        } finally {
            isLoading = false;
        }
    }
</script>

<div class="min-h-screen flex items-center justify-center bg-gray-100 p-4">
    <div class="max-w-md w-full bg-white rounded-xl shadow-md overflow-hidden">
        
        <div class="flex border-b border-gray-200">
            <button 
                class="flex-1 py-4 text-sm font-bold uppercase tracking-wider transition-colors {activeTab === 'login' ? 'bg-white text-slate-900 border-b-2 border-slate-900' : 'bg-gray-50 text-gray-500 hover:bg-gray-100'}"
                onclick={() => { activeTab = 'login'; errorMessage = ''; successMessage = ''; }}>
                Logga in
            </button>
            <button 
                class="flex-1 py-4 text-sm font-bold uppercase tracking-wider transition-colors {activeTab === 'register' ? 'bg-white text-slate-900 border-b-2 border-slate-900' : 'bg-gray-50 text-gray-500 hover:bg-gray-100'}"
                onclick={() => { activeTab = 'register'; errorMessage = ''; successMessage = ''; }}>
                Registrera
            </button>
        </div>

        <div class="p-8">
            <div class="text-center mb-6">
                <h1 class="text-3xl font-black tracking-wider uppercase text-slate-900">Läktaren</h1>
                <p class="text-gray-500 mt-2">
                    {activeTab === 'login' ? 'Visa din biljett i vändkorset' : 'Skapa ditt konto och anslut till klacken'}
                </p>
            </div>

            {#if errorMessage}
                <div class="bg-red-50 text-red-700 p-3 rounded-lg mb-4 text-sm font-medium border border-red-200">{errorMessage}</div>
            {/if}
            {#if successMessage}
                <div class="bg-green-50 text-green-700 p-3 rounded-lg mb-4 text-sm font-medium border border-green-200">{successMessage}</div>
            {/if}

            {#if activeTab === 'login'}
                <form onsubmit={handleLogin} class="space-y-4">
                    <div>
                        <label for="login-email" class="block text-sm font-medium text-gray-700 mb-1">E-postadress</label>
                        <input type="email" id="login-email" bind:value={loginEmail} required 
                            class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900" 
                            placeholder="din@epost.se" />
                    </div>
                    <div>
                        <label for="login-password" class="block text-sm font-medium text-gray-700 mb-1">Lösenord</label>
                        <input type="password" id="login-password" bind:value={loginPassword} required 
                            class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900" 
                            placeholder="Ditt hemliga lösenord" />
                    </div>
                    <button type="submit" disabled={isLoading} 
                        class="w-full bg-slate-900 text-white font-bold py-3 rounded-lg hover:bg-slate-800 transition-colors disabled:opacity-50 mt-4">
                        {isLoading ? 'Verifierar...' : 'Logga in'}
                    </button>
                </form>
            {/if}

            {#if activeTab === 'register'}
                <form onsubmit={handleRegister} class="space-y-4">
                    <div>
                        <label for="reg-username" class="block text-sm font-medium text-gray-700 mb-1">Användarnamn</label>
                        <input type="text" id="reg-username" bind:value={regUsername} required 
                            class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900" 
                            placeholder="T.ex. Kuban1908" />
                    </div>
                    <div>
                        <label for="reg-email" class="block text-sm font-medium text-gray-700 mb-1">E-postadress</label>
                        <input type="email" id="reg-email" bind:value={regEmail} required 
                            class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900" 
                            placeholder="din@epost.se" />
                    </div>
                    <div>
                        <label for="reg-password" class="block text-sm font-medium text-gray-700 mb-1">Lösenord</label>
                        <input type="password" id="reg-password" bind:value={regPassword} required minlength="6"
                            class="w-full p-3 bg-gray-50 border border-gray-200 rounded-lg focus:outline-none focus:ring-2 focus:ring-slate-900" 
                            placeholder="Minst 6 tecken" />
                    </div>
                    <button type="submit" disabled={isLoading} 
                        class="w-full bg-slate-900 text-white font-bold py-3 rounded-lg hover:bg-slate-800 transition-colors disabled:opacity-50 mt-4">
                        {isLoading ? 'Registrerar...' : 'Skapa konto'}
                    </button>
                </form>
            {/if}

        </div>
    </div>
</div>