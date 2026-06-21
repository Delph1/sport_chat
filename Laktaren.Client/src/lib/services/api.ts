const API_BASE_URL = 'http://localhost:5253/api';

export async function getPosts() {
    try {
        const response = await fetch(`${API_BASE_URL}/posts`);
    if (!response.ok) throw new Error('Error when fetching posts');
    return await response.json();
    } catch (error) {
        console.error("Error when fetch posts from the backend.");
        return [];
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

export async function createPost(postData: any) {
    const token = localStorage.getItem('token');

    return await fetch(`${API_BASE_URL}/posts`, {
        method: 'POST',
        headers: { 
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}` 
        },
        body: JSON.stringify(postData)
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