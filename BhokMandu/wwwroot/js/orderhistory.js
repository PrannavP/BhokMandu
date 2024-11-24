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
                    // Trigger success toast
                    const toastElement = document.getElementById('statusToast');
                    const toast = new bootstrap.Toast(toastElement, {
                        delay: 3000 // Toast will disappear after 3 seconds
                    });
                    toast.show();
                } else {
                    return response.json().then(err => {
                        throw new Error(err.message);
                    });
                }
            })
            .catch(error => {
                console.error('Error:', error);
                // Trigger error toast
                const toastElement = document.getElementById('statusToast');
                const toast = new bootstrap.Toast(toastElement, {
                    delay: 3000
                });
                document.getElementById('statusToast').querySelector('.toast-body').textContent = 'Error updating order status: ' + error.message;
                toast.show();
            });
    };
});