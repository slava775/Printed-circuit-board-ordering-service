const API_BASE_URL = "https://localhost:7141/api";
const FILES_BASE_URL = "https://localhost:7141";
const API_AUTH_URL = `${API_BASE_URL}/auth`;

async function apiRequest(url, options = {}) {
    const token = localStorage.getItem("accessToken");

    const defaultOptions = {
        headers: {
            "Content-Type": "application/json",
            ...(token && { "Authorization": `Bearer ${token}` })
        }
    };

    const mergedOptions = {
        ...defaultOptions,
        ...options,
        headers: {
            ...defaultOptions.headers,
            ...options.headers
        }
    };

    const response = await fetch(url, mergedOptions);

    const text = await response.text();
    let data = null;

    if (text) {
        try {
            data = JSON.parse(text);
        } catch (e) {}
    }

    if (!response.ok) {
        throw new Error(data?.message || `HTTP Error: ${response.status}`);
    }

    return data;
}

function validateEmail(email) {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
}