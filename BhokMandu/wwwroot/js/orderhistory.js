document.addEventListener('DOMContentLoaded', () => {
    const updateOrderStatusUrl = document.getElementById("updateOrderStatusUrl").value;

    window.updateStatus = function (orderId) {
        const selectedStatus = document.getElementById("status_" + orderId).value;
        const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

        const data = {
            orderId: orderId,
            status: selectedStatus
        };

        fetch(updateOrderStatusUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': token
            },
            body: JSON.stringify(data)
        })
            .then(response => {
                if (response.ok) {
                    alert('Order status updated successfully.');
                } else {
                    return response.json().then(err => {
                        throw new Error(err.message);
                    });
                }
            })
            .catch(error => {
                alert('Error updating order status: ' + error.message);
                console.error('Error:', error);
            });
    };
});
