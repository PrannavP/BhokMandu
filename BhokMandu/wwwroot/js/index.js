document.addEventListener('DOMContentLoaded', function () {
    const cartCount = localStorage.getItem('cartCount') || 0;
    updateCartCountDisplay(cartCount); // Update badge with cart count
});

function addToCart(foodItem, price, foodId) {
    const quantityInput = document.getElementById(`quantity-${foodId}`);
    const quantity = parseInt(quantityInput.value) || 1; // Default to 1 if input is invalid

    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let itemCount = 0;

    const existingItem = cart.find(item => item.FoodName === foodItem);

    if (existingItem) {
        existingItem.Quantity += quantity; // Increase quantity
    } else {
        // Add new item
        cart.push({ FoodName: foodItem, Quantity: quantity, Price: price });
    }

    localStorage.setItem('cart', JSON.stringify(cart));

    // Calculate total items in the cart
    itemCount = cart.reduce((total, item) => total + item.Quantity, 0);
    localStorage.setItem('cartCount', itemCount);

    // Update displayed count in navbar
    updateCartCountDisplay(itemCount); // Call to update display

    // Show the modal with the cart message
    showCartModal(foodItem, quantity);
}

// Function to increase quantity
function increaseQuantity(foodId) {
    const quantityInput = document.getElementById(`quantity-${foodId}`);
    let quantity = parseInt(quantityInput.value) || 0;
    quantityInput.value = quantity + 1; // Increase by 1
}

// Function to decrease quantity
function decreaseQuantity(foodId) {
    const quantityInput = document.getElementById(`quantity-${foodId}`);
    let quantity = parseInt(quantityInput.value) || 0;
    if (quantity > 1) {
        quantityInput.value = quantity - 1; // Decrease by 1, but not below 1
    }
}

// Function to show the cart modal
function showCartModal(foodItem, quantity) {
    const cartModalMessage = document.getElementById("cartModalMessage");
    cartModalMessage.textContent = `${quantity} x ${foodItem} has been added to your cart!`;

    const modal = document.getElementById("cartModal");
    modal.style.display = 'block'; // Show the modal
}

// Function to close the modal
function closeModal() {
    const modal = document.getElementById("cartModal");
    modal.style.display = 'none'; // Hide the modal
}

// Function to update displayed cart count in navbar
function updateCartCountDisplay(count) {
    const cartCountElement = document.getElementById('cart-count-badge');
    if (cartCountElement) {
        cartCountElement.textContent = count; // Update text content with new count
    }
}