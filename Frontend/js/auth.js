// ===== АВТОРИЗАЦИЯ =====
function checkAuth() {
    const token = localStorage.getItem("accessToken");
    const authDiv = document.getElementById("authButtons");
    
    if (!authDiv) return;
    
    if (token) {
        let userName = "Пользователь";
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            if (payload.name) userName = payload.name;
        } catch (e) {}
        
        authDiv.innerHTML = `
            <div class="user-info">
                <span class="user-name" onclick="window.location.href='../pages/home.html'">${userName}</span>
                <button class="logout-btn" onclick="logout()">Выйти</button>
            </div>
        `;
    } else {
        // Определяем, на какой странице мы находимся, чтобы ссылки вели правильно
        const currentPage = window.location.pathname.split('/').pop();
        
        if (currentPage === 'login.html' || currentPage === 'register.html' || currentPage === 'confirm.html') {
            // На страницах авторизации показываем только кнопки (ссылки не нужны, они уже на своих страницах)
            authDiv.innerHTML = `
                <button class="btn-header" onclick="window.location.href='login.html'">Вход</button>
                <button class="btn-header" onclick="window.location.href='register.html'">Регистрация</button>
            `;
        } else {
            authDiv.innerHTML = `
                <button class="btn-header" onclick="window.location.href='../pages/login.html'">Вход</button>
                <button class="btn-header" onclick="window.location.href='../pages/register.html'">Регистрация</button>
            `;
        }
    }
}

async function logout() {
    const refreshToken = localStorage.getItem("refreshToken");
    
    try {
        await apiRequest(`${API_AUTH_URL}/logout`, {
            method: "POST",
            body: JSON.stringify(refreshToken)
        });
    } catch (e) {
        console.warn("Ошибка при выходе:", e);
    } finally {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = "../pages/index.html";
    }
}

async function login(email, password) {
    const data = await apiRequest(`${API_AUTH_URL}/login`, {
        method: "POST",
        body: JSON.stringify({ email, password })
    });
    
    localStorage.setItem("accessToken", data.accessToken);
    localStorage.setItem("refreshToken", data.refreshToken);
    window.location.href = "../pages/home.html";
}

async function register(name, surname, email, password, idCountry) {
    await apiRequest(`${API_AUTH_URL}/registr`, {
        method: "POST",
        body: JSON.stringify({ name, surname, email, password, idCountry: parseInt(idCountry) })
    });
    
    localStorage.setItem("pendingEmail", email);
    window.location.href = "../pages/confirm.html";
}

async function confirmEmail(email, code) {
    const data = await apiRequest(`${API_AUTH_URL}/confirm-email?email=${encodeURIComponent(email)}&code=${code}`, {
        method: "POST"
    });
    
    if (data?.accessToken) localStorage.setItem("accessToken", data.accessToken);
    if (data?.refreshToken) localStorage.setItem("refreshToken", data.refreshToken);
    localStorage.removeItem("pendingEmail");
    
    return data;
}

async function resendCode(email) {
    await apiRequest(`${API_AUTH_URL}/send-code`, {
        method: "POST",
        body: JSON.stringify(email)
    });
}

async function loadCountries() {
    try {
        const response = await fetch(`${API_AUTH_URL}/countries`);
        const countries = await response.json();
        const select = document.getElementById("regCountry");
        
        if (!select) return;
        
        select.innerHTML = '<option value="">Выберите страну</option>';
        countries.forEach(country => {
            const option = document.createElement("option");
            option.value = country.idCountry;
            option.textContent = country.name;
            select.appendChild(option);
        });
    } catch (error) {
        console.error("Ошибка загрузки стран:", error);
    }
}