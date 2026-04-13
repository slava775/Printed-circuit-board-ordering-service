let allProducts = [];

async function loadProducts() {
    const container = document.getElementById("productsContainer");
    if (!container) return;
    
    try {
        const products = await apiRequest(`${API_BASE_URL}/products`);
        allProducts = products;
        
        if (allProducts.length === 0) {
            container.innerHTML = '<div class="no-results">Товары временно недоступны</div>';
            return;
        }
        
        renderProducts(allProducts);
    } catch (error) {
        console.error("Ошибка загрузки товаров:", error);
        container.innerHTML = '<div class="no-results">Ошибка загрузки товаров</div>';
    }
}

function renderProducts(products) {
    const container = document.getElementById("productsContainer");
    if (!container) return;
    
    if (products.length === 0) {
        container.innerHTML = '<div class="no-results">Нет товаров, соответствующих фильтру</div>';
        return;
    }
    
    container.innerHTML = products.map(product => {
        const spec = product.specification || {};
        const specs = [];
        
        if (spec.layers) specs.push(`${spec.layers} слоя`);
        if (spec.thickness) specs.push(`${spec.thickness} мм`);
        if (spec.material) specs.push(spec.material);
        
        return `
            <div class="product-card">
                <div class="product-image">PCB</div>
                <div class="product-info">
                    <div class="product-title">${product.name || 'Печатная плата'}</div>
                    <div class="product-description">${product.description || 'Качественная печатная плата для ваших проектов'}</div>
                    <div class="product-specs">
                        ${specs.map(s => `<span class="spec-tag">${s}</span>`).join('')}
                    </div>
                    <div class="product-price">${(product.price || 0).toLocaleString('ru-RU')} ₽</div>
                    <button class="btn-order" onclick="orderProduct(${product.idProduct || product.id})">Заказать</button>
                </div>
            </div>
        `;
    }).join('');
}

function filterProducts(layers, event) {
    if (event) {
        document.querySelectorAll('.filter-btn').forEach(b => b.classList.remove('active'));
        event.target.classList.add('active');
    }
    
    if (layers === 'all') {
        renderProducts(allProducts);
    } else {
        const filtered = allProducts.filter(p => {
            const spec = p.specification || {};
            return spec.layers === parseInt(layers);
        });
        renderProducts(filtered);
    }
}

function orderProduct(productId) {
    const token = localStorage.getItem("accessToken");
    if (!token) {
        if (confirm("Для заказа необходимо войти в аккаунт. Перейти на страницу входа?")) {
            window.location.href = "pages/login.html";
        }
        return;
    }
    alert(`Товар #${productId} добавлен в корзину. Функция в разработке.`);
}

function initFilters() {
    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            filterProducts(btn.dataset.filter, e);
        });
    });
}