// ===== SHOPPING CART FUNCTIONS =====

// Add to cart functionality
document.addEventListener('DOMContentLoaded', function () {
    // Load cart count on page load
    updateCartBadge();

    // Add to cart button click handlers
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    addToCartButtons.forEach(button => {
        button.addEventListener('click', function (e) {
            e.preventDefault();

            const productId = this.dataset.productId;
            const productName = this.dataset.productName;

            addToCart(productId, 1, productName);
        });
    });
});

// Add product to cart via AJAX
function addToCart(productId, quantity, productName) {
    fetch('/Cart/AddToCart', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `productId=${productId}&quantity=${quantity}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Show success notification
                showNotification(`✓ Đã thêm ${productName} vào giỏ hàng`, 'success');

                // Update cart badge
                updateCartBadge(data.cartCount);
            } else {
                showNotification('✗ ' + data.message, 'error');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showNotification('✗ Đã xảy ra lỗi', 'error');
        });
}

// Update cart badge count
function updateCartBadge(count) {
    const badge = document.getElementById('cartBadge');
    if (!badge) return;

    if (count === undefined) {
        // Fetch cart count from server
        let cartCount = 0;
        // Parse from session if available (simplified)
        // In real app, make AJAX call to get cart count
        cartCount = 0; // Will be updated when items are added

        if (cartCount > 0) {
            badge.textContent = cartCount;
            badge.style.display = 'inline-block';
        } else {
            badge.style.display = 'none';
        }
    } else {
        if (count > 0) {
            badge.textContent = count;
            badge.style.display = 'inline-block';
        } else {
            badge.style.display = 'none';
        }
    }
}

// Show notification toast
function showNotification(message, type) {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `cart-notification ${type}`;
    notification.textContent = message;

    // Add to page
    document.body.appendChild(notification);

    // Show notification
    setTimeout(() => {
        notification.classList.add('show');
    }, 10);

    // Hide and remove after 3 seconds
    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => {
            notification.remove();
        }, 300);
    }, 3000);
}

// Update quantity in cart page
function updateCartQuantity(productId, quantity) {
    fetch('/Cart/UpdateQuantity', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `productId=${productId}&quantity=${quantity}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Reload page to update totals
                location.reload();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showNotification('✗ Đã xảy ra lỗi', 'error');
        });
}

// Remove item from cart
function removeCartItem(productId) {
    if (!confirm('Bạn có chắc muốn xóa món này khỏi giỏ hàng?')) {
        return;
    }

    fetch('/Cart/RemoveItem', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `productId=${productId}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showNotification(data.message, 'success');
                // Reload page
                location.reload();
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showNotification('✗ Đã xảy ra lỗi', 'error');
        });
}
