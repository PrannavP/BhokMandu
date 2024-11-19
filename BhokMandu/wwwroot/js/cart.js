document.addEventListener('DOMContentLoaded', function () {
    // Retrieve cart data from local storage
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    const cartItemsContainer = document.getElementById('cart-items');
    let totalAmount = 0;

    // Check if the cart is empty
    if (cart.length === 0) {
        cartItemsContainer.innerHTML = '<p>Your cart is empty.</p>';
        return;
    }

    // Loop through each item in the cart and display it
    cart.forEach(item => {
        const itemTotal = item.quantity * item.price; // Use item's price for total calculation
        totalAmount += itemTotal;

        const itemElement = document.createElement('div');
        itemElement.classList.add('cart-item');
        itemElement.innerHTML = `
                <h5>${item.name}</h5>
                <p>Quantity: ${item.quantity}</p>
                <p>Price per unit: Rs. ${item.price}</p>
                <p>Total: Rs. ${itemTotal}</p>
                <hr />
            `;
        cartItemsContainer.appendChild(itemElement);
    });

    // Update total amount in the DOM
    document.getElementById('total-amount').textContent = totalAmount;
});