// cart.js
function addToCart(foodItem, price, foodId) {
    // Get the quantity from the input field
    const quantityInput = document.getElementById(`quantity-${foodId}`);
    const quantity = parseInt(quantityInput.value) || 1; // Default to 1 if input is invalid

    // Get existing cart items from local storage
    let cart = JSON.parse(localStorage.getItem('cart')) || [];
    let itemCount = 0;

    // Check if the item is already in the cart
    const existingItem = cart.find(item => item.name === foodItem);

    if (existingItem) {
        // If it exists, increase the quantity
        existingItem.quantity += quantity;
    } else {
        // If it doesn't exist, add it to the cart with its price
        cart.push({ name: foodItem, quantity: quantity, price: price });
    }

    // Save the updated cart back to local storage
    localStorage.setItem('cart', JSON.stringify(cart));

    // Calculate total items in the cart
    itemCount = cart.reduce((total, item) => total + item.quantity, 0);

    // Save total item count to local storage
    localStorage.setItem('cartCount', itemCount);

    // Update navbar cart count display
    updateCartCountDisplay(itemCount); // Call to update the display

    alert(`${quantity} x ${foodItem} added to the cart!`);
}

// Function to update the displayed cart count in the navbar
function updateCartCountDisplay(count) {
    const cartCountElement = document.querySelector('.navbar .badge');
    if (cartCountElement) {
        cartCountElement.textContent = count; // Update text content with new count
    }
}

// Update count on page load
document.addEventListener('DOMContentLoaded', function () {
    const cartCount = localStorage.getItem('cartCount') || 0;
    updateCartCountDisplay(cartCount);
});