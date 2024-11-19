function displayCartItems() {
    const cart = JSON.parse(localStorage.getItem('cart')) || [];
    const cartItemsContainer = document.getElementById('cart-items');
    let totalAmount = 0;

    // Clear previous cart items
    cartItemsContainer.innerHTML = ''; // Clear existing items

    // Check if the cart is empty
    if (cart.length === 0) {
        cartItemsContainer.innerHTML = '<p>Your cart is empty.</p>';
        document.getElementById('total-amount').textContent = '0'; // Set total to 0
        return;
    }

    // Loop through each item in the cart and display it
    cart.forEach(item => {
        const itemTotal = item.Quantity * item.Price; // Calculate total for this item
        totalAmount += itemTotal; // Add to overall total

        const itemElement = document.createElement('div');
        itemElement.classList.add('cart-item');
        itemElement.innerHTML = `
            <h5>${item.FoodName}</h5>
            <p>Quantity: ${item.Quantity}</p>
            <p>Price per unit: Rs. ${item.Price}</p>
            <p>Total: Rs. ${itemTotal}</p>
            <hr />
        `;
        cartItemsContainer.appendChild(itemElement);
    });

    // Update total amount in the DOM
    document.getElementById('total-amount').textContent = totalAmount.toFixed(2); // Format total to 2 decimal places
}