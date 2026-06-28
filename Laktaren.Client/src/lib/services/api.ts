const API_BASE_URL = 'http://localhost:5253/api';

export async function getPosts() {
    try {
        const response = await fetch(`${API_BASE_URL}/posts`);
        return response; 
    } catch (error) {
        console.error("Kunde inte nå backend:", error);
        return { ok: false }; 
    }
}

export async function registerUser(userData: any) {
    return await fetch(`${API_BASE_URL}/users/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(userData)
    });
}

export async function loginUser(credentials: any) {
    return await fetch(`${API_BASE_URL}/users/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(credentials)
    });
}

export async function createPost(post: any) {
    const token = localStorage.getItem('token');

    return await fetch(`${API_BASE_URL}/posts`, {
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        },
        body: JSON.stringify(post)
    });
}

export async function toggleReaction(postId: string) {
    const token = localStorage.getItem('token');

    return await fetch(`${API_BASE_URL}/reactions/${postId}`, {
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        }
    });
}

export async function loadReplies(postId: string) {
    try{
        return await fetch(`${API_BASE_URL}/posts/${postId}/replies/`);
    }
    catch (error)
    {
        console.error("Kunde inte nå backend:", error);
        return { ok: false }; 
    }
}

export async function getTeams() {
    try
    {
        return await fetch(`${API_BASE_URL}/teams/`);
    }
    catch (error)
    {
        console.log("Kunde inte hämta lag:", error);
        return { ok: false }; 
    }
}

export async function saveUserPreferences(preferences: { teamId: string, secondaryTeamIds: string[], useTeamColors: boolean }) {
    try {
        return await fetch(`${API_BASE_URL}/users/preferences`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${localStorage.getItem('token')}`
            },
            body: JSON.stringify(preferences)
        });
    } catch (error) {
        console.error("Kunde inte spara preferenser:", error);
        return { ok: false };
    }
}

export async function getMyProfile() {
    try {
        return await fetch(`${API_BASE_URL}/users/me`, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${localStorage.getItem('token')}`,
                'Content-Type': 'application/json'
            }
        });
    } catch (error) {
        console.error("Kunde inte hämta profil:", error);
        return { ok: false };
    }
}