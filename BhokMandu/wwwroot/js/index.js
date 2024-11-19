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

    alert(`${quantity} x ${foodItem} added to the cart!`);
}

// Function to update displayed cart count in navbar
function updateCartCountDisplay(count) {
    const cartCountElement = document.getElementById('cart-count-badge');
    if (cartCountElement) {
        cartCountElement.textContent = count; // Update text content with new count
    }
}