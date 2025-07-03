const productContainer = document.querySelector('.shopContainer');
const productDetailPage = document.querySelector('.proDetails');
const cartPage = document.querySelector('.cart');

if (productContainer) {
    displayProducts();
}
else if (productDetailPage) {
    displayProductDetail();
} else if (cartPage) {
    displayCart();
}


/* Product Function */

function displayProducts() {
    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.classList.add('pro');
        productCard.innerHTML = `
            <div class="mainImage">
                <img src="${product.imageURL}" alt="${product.name}">
            </div>
            <h5 class="name">${product.name}</h5>
            <h4 class="price">$${product.price}</h4>
        `;
        productContainer.appendChild(productCard);

        productCard.addEventListener('click', () => {
            sessionStorage.setItem('selectedProduct', JSON.stringify(product));
            window.location.href = '/Products/Details'; // Make sure this route works
        });
    });
}



/* Product Detail Page Function */

function displayProductDetail() {
    const productData = JSON.parse(sessionStorage.getItem('selectedProduct'));

    document.querySelector('.name').innerText = productData.name;
    document.querySelector('.price').innerText = `$${productData.price}`;
    document.querySelector('.camera').innerText = productData.camera;
    document.querySelector('.storage').innerText = productData.storage;
    document.querySelector('.battery').innerText = productData.battery;
    document.querySelector('.color').innerText = productData.color; // ? used 'colour'
    document.querySelector('.description').innerText = productData.description;


    function updateProductDisplay(productData) {
        const mainImageEl = document.querySelector('.mainImage1');
        const smallImageEl = document.querySelector('.smallImageGroup');

        // Clear existing thumbnails
        smallImageEl.innerHTML = "";

        // Create main image
        const mainImg = document.createElement('img');
        mainImg.src = productData.imageURL;
        mainImg.alt = productData.name;
        mainImg.classList.add('mainImage1');

        // Remove existing image if any
        const existingImg = mainImageEl.querySelector('img');
        if (existingImg) {
            existingImg.remove();
        }

        // Insert the new main image
        mainImageEl.insertBefore(mainImg, smallImageEl);

        // If productData.smallImageGroup exists, render thumbnails
        const allImages = productData.smallImageGroup?.slice(0, 4) || [];

        allImages.forEach(image => {
            const img = document.createElement('img');
            img.src = image;
            img.alt = productData.name;
            img.classList.add('thumbnail');

            img.addEventListener('click', () => {
                mainImg.src = image;
            });

            smallImageEl.appendChild(img);
        });
    }

function addCart(product) {
    let cart = JSON.parse(sessionStorage.getItem('cart')) || [];

    const existingItem = cart.find(item => item.id === product.id);
    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({
            id: product.id,
            name: product.name,
            price: parseFloat(product.price),          // ? Ensure it's a number
            imageURL: product.imageURL,                // ? Use imageURL
            storage: product.storage,
            color: product.color,                      // ? Use color not colour
            quantity: 1
        });
    }

    sessionStorage.setItem('cart', JSON.stringify(cart));
    updateCartBadge();
}



/* Cart Function */

    function displayCart() {
        const cart = JSON.parse(sessionStorage.getItem('cart')) || [];

        const cartItemsEl = document.querySelector('.cartItems');
        const subtotalEl = document.querySelector('.subtotal');
        const totalEl = document.querySelector('.totalPrice');

        cartItemsEl.innerHTML = '';

        if (cart.length === 0) {
            cartItemsEl.innerHTML = '<p class="emptyCart">Your cart is empty</p>';
            subtotalEl.textContent = '$0';
            totalEl.textContent = '$0';
            return;
        }

        let subtotal = 0;

        cart.forEach((item, index) => {
            const itemTotal = item.price * item.quantity;
            subtotal += itemTotal;

            const cartItem = document.createElement('div');
            cartItem.classList.add('item');
            cartItem.innerHTML = `
            <div class="cartProduct">
                <img src="${item.imageURL}" alt="${item.name}">
                <div class="cartProductInfo">
                    <p>${item.name}</p>
                    <p>${item.storage}, ${item.color}</p>
                </div>
            </div>
            <p class="cartPrice">$${item.price}</p>
            <div class="cartQuantity">
                <input type="number" value="${item.quantity}" min="1" data-index="${index}">
            </div>
            <p class="cartTotal">$${itemTotal.toFixed(2)}</p>
            <p class="removeItem" data-index="${index}">
                <i class="far fa-times-circle"></i>
            </p>
        `;
            cartItemsEl.appendChild(cartItem);
        });

        subtotalEl.textContent = `$${subtotal.toFixed(2)}`;
        totalEl.textContent = `$${subtotal.toFixed(2)}`;

        removeCartItem();
        updateCartQuantity();
    }


    function removeCartItem() {
        document.querySelectorAll('.removeItem').forEach(button => {
            button.addEventListener('click', function () {
                let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
                const index = this.getAttribute('data-index');

                cart.splice(index, 1);
                sessionStorage.setItem('cart', JSON.stringify(cart));
                displayCart();
                updateCartBadge();
            });
        });
    }


    function updateCartQuantity() {
        document.querySelectorAll('.cartQuantity input').forEach(input => {
            input.addEventListener('change', function () {
                let cart = JSON.parse(sessionStorage.getItem('cart')) || [];
                const index = this.getAttribute('data-index');

                let quantity = parseInt(this.value);
                if (isNaN(quantity) || quantity < 1) {
                    quantity = 1;
                    this.value = 1; // Reset visually
                }

                cart[index].quantity = quantity;
                sessionStorage.setItem('cart', JSON.stringify(cart));
                displayCart();
                updateCartBadge();
            });
        });
    }

    function updateCartBadge() {
        const cart = JSON.parse(sessionStorage.getItem('Cart')) || [];
        const cartCount = cart.reduce((total, item) => total + item.quantity, 0);
        const badge = document.querySelector('.cartCount');

        if (badge) {
            if (cartCount > 0) {
                badge.textContent = cartCount;
                badge.style.display = 'block';
            } else {
                badge.style.display = 'none';
            }
        }
    }



// Payment

const checkoutButton = document.querySelector(".btn");
const payment = document.querySelector(".payment");
const close = document.querySelector(".close");

checkoutButton.addEventListener("click", () => {
    payment.style.display = "flex";
    document.body.style.overflow = 'hidden'; // THIS disables scrolling
});

close.addEventListener("click", () => {
    payment.style.display = "none";
    document.body.style.overflow = 'auto';  // THIS enables scrolling back
});
