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

        const productName = (product.name || 'Печатная плата').replace(/'/g, "\\'");
        
        return `
            <div class="product-card">
                <div class="product-image" onclick="window.location.href='product-details.html?id=${product.idProduct}'" style="cursor:pointer;">PCB</div>
                <div class="product-info">
                    <div class="product-title" onclick="window.location.href='product-details.html?id=${product.idProduct}'" style="cursor:pointer;">${product.name || 'Печатная плата'}</div>
                    <div class="product-description">${product.description || 'Качественная печатная плата для ваших проектов'}</div>
                    <div class="product-specs">
                        ${specs.map(s => `<span class="spec-tag">${s}</span>`).join('')}
                    </div>
                    <div class="product-price">${(product.price || 0).toLocaleString('ru-RU')} ₽</div>
                    <button class="btn-order" onclick="window.location.href='product-details.html?id=${product.idProduct}'">Подробнее</button>
            </div>
        `;
    }).join('');
}

function addToCart(idProduct, name, price) {
    let cart = localStorage.getItem("cart");
    cart = cart ? JSON.parse(cart) : [];
    
    const existing = cart.find(item => item.idProduct === idProduct);
    if (existing) {
        existing.quantity += 1;
    } else {
        cart.push({
            idProduct: idProduct,
            name: name,
            price: price,
            quantity: 1
        });
    }
    
    localStorage.setItem("cart", JSON.stringify(cart));
    alert(`Товар "${name}" добавлен в корзину`);
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

function initFilters() {
    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            filterProducts(btn.dataset.filter, e);
        });
    });
}