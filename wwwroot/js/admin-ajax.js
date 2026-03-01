/**
 * admin-ajax.js – Shared AJAX utilities for O'BeefSoup Admin
 * Features: AJAX delete, search/filter, client-side pagination, order status update, toast
 */
(function () {
    'use strict';

    /* =====================================================================
       TOAST NOTIFICATION
    ===================================================================== */
    function showToast(message, type) {
        type = type || 'success';
        const container = document.getElementById('adminToastContainer');
        if (!container) return;

        const id = 'toast_' + Date.now();
        const bgClass = type === 'success' ? 'bg-success' : type === 'danger' ? 'bg-danger' : 'bg-warning';
        const icon = type === 'success' ? 'bi-check-circle-fill' : type === 'danger' ? 'bi-x-circle-fill' : 'bi-exclamation-triangle-fill';

        const html = `
        <div id="${id}" class="toast align-items-center text-white ${bgClass} border-0 shadow-lg" role="alert" aria-live="assertive" aria-atomic="true">
            <div class="d-flex">
                <div class="toast-body d-flex align-items-center gap-2">
                    <i class="bi ${icon}"></i> ${message}
                </div>
                <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
            </div>
        </div>`;

        container.insertAdjacentHTML('beforeend', html);
        const toastEl = document.getElementById(id);
        const bsToast = new bootstrap.Toast(toastEl, { delay: 4000 });
        bsToast.show();
        toastEl.addEventListener('hidden.bs.toast', () => toastEl.remove());
    }

    // Expose globally
    window.showToast = showToast;

    /* =====================================================================
       CONFIRM DELETE MODAL
    ===================================================================== */
    function initDeleteModal() {
        if (document.getElementById('adminDeleteModal')) return;
        const html = `
        <div class="modal fade" id="adminDeleteModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content border-0 shadow">
                    <div class="modal-header border-0 pb-0">
                        <div class="d-flex align-items-center gap-3">
                            <div class="rounded-circle d-flex align-items-center justify-content-center"
                                 style="width:46px;height:46px;background:#fff0f0;">
                                <i class="bi bi-trash3-fill text-danger fs-5"></i>
                            </div>
                            <div>
                                <h5 class="modal-title fw-bold mb-0">Xác nhận xóa</h5>
                                <p class="text-muted small mb-0" id="adminDeleteModalMsg">Hành động này không thể hoàn tác.</p>
                            </div>
                        </div>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-footer border-0 pt-2">
                        <button type="button" class="btn btn-light border" data-bs-dismiss="modal">Hủy</button>
                        <button type="button" class="btn btn-danger" id="adminDeleteConfirmBtn">
                            <i class="bi bi-trash me-1"></i> Xóa
                        </button>
                    </div>
                </div>
            </div>
        </div>`;
        document.body.insertAdjacentHTML('beforeend', html);
    }

    /**
     * confirmDelete – show modal and AJAX delete on confirm
     * @param {string} url          POST URL for DeleteAjax
     * @param {string} token        AntiForgery token value
     * @param {string} rowId        data-row-id value of the <tr> or card element
     * @param {string} containerId  id of container to refresh (optional)
     * @param {string} partialUrl   URL to refresh container with partial view (optional)
     * @param {string} message      Custom confirmation message
     */
    function confirmDelete(url, token, rowId, containerId, partialUrl, message) {
        initDeleteModal();

        const msgEl = document.getElementById('adminDeleteModalMsg');
        if (msgEl) msgEl.textContent = message || 'Bạn có chắc muốn xóa mục này? Hành động không thể hoàn tác.';

        const modal = new bootstrap.Modal(document.getElementById('adminDeleteModal'));
        modal.show();

        const confirmBtn = document.getElementById('adminDeleteConfirmBtn');
        // Remove old listener
        const newBtn = confirmBtn.cloneNode(true);
        confirmBtn.parentNode.replaceChild(newBtn, confirmBtn);

        newBtn.addEventListener('click', function () {
            newBtn.disabled = true;
            newBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang xóa...';

            fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: '__RequestVerificationToken=' + encodeURIComponent(token)
            })
            .then(r => r.json())
            .then(data => {
                modal.hide();
                if (data.success) {
                    showToast(data.message || 'Xóa thành công!', 'success');
                    if (rowId) {
                        const row = document.querySelector('[data-row-id="' + rowId + '"]');
                        if (row) {
                            row.style.transition = 'opacity 0.3s, height 0.3s';
                            row.style.opacity = '0';
                            setTimeout(() => row.remove(), 300);
                        }
                    }
                    if (containerId && partialUrl) {
                        refreshContainer(containerId, partialUrl);
                    }
                } else {
                    showToast(data.message || 'Xóa thất bại!', 'danger');
                }
            })
            .catch(() => {
                modal.hide();
                showToast('Lỗi kết nối, vui lòng thử lại!', 'danger');
            })
            .finally(() => {
                newBtn.disabled = false;
                newBtn.innerHTML = '<i class="bi bi-trash me-1"></i> Xóa';
            });
        });
    }

    window.confirmDelete = confirmDelete;

    /* =====================================================================
       AJAX SEARCH / FILTER
    ===================================================================== */
    /**
     * initAjaxSearch – intercept a form's submit and load partial view into container
     * @param {string} formId       id of the search/filter form
     * @param {string} containerId  id of the container to update
     * @param {string} partialUrl   base URL for the partial-view action
     */
    function initAjaxSearch(formId, containerId, partialUrl) {
        const form = document.getElementById(formId);
        const container = document.getElementById(containerId);
        if (!form || !container) return;

        function loadResults() {
            const params = new URLSearchParams(new FormData(form)).toString();
            const url = partialUrl + (params ? '?' + params : '');
            container.style.opacity = '0.5';
            container.style.pointerEvents = 'none';

            fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
                .then(r => r.text())
                .then(html => {
                    container.innerHTML = html;
                    container.style.opacity = '1';
                    container.style.pointerEvents = '';
                    // Reinit pagination if present
                    const pager = container.querySelector('[data-page-size]');
                    if (pager) initClientPagination(containerId, parseInt(pager.dataset.pageSize));
                })
                .catch(() => {
                    container.style.opacity = '1';
                    container.style.pointerEvents = '';
                    showToast('Lỗi tải dữ liệu!', 'danger');
                });
        }

        form.addEventListener('submit', function (e) {
            e.preventDefault();
            loadResults();
        });

        // Clear/reset button
        const clearBtns = form.querySelectorAll('[data-ajax-clear]');
        clearBtns.forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.preventDefault();
                form.reset();
                loadResults();
            });
        });
    }

    window.initAjaxSearch = initAjaxSearch;

    /* =====================================================================
       REFRESH CONTAINER (load partial into div)
    ===================================================================== */
    function refreshContainer(containerId, partialUrl, params) {
        const container = document.getElementById(containerId);
        if (!container) return;
        const url = partialUrl + (params ? '?' + params : '');
        container.style.opacity = '0.5';
        fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } })
            .then(r => r.text())
            .then(html => {
                container.innerHTML = html;
                container.style.opacity = '1';
                const pager = container.querySelector('[data-page-size]');
                if (pager) initClientPagination(containerId, parseInt(pager.dataset.pageSize));
            })
            .catch(() => { container.style.opacity = '1'; });
    }

    window.refreshContainer = refreshContainer;

    /* =====================================================================
       CLIENT-SIDE PAGINATION
    ===================================================================== */
    /**
     * initClientPagination – paginate rows inside a table (tbody) or card list
     * @param {string} containerId  id of container with table or .paginate-item elements
     * @param {number} pageSize     rows per page (default 10)
     */
    function initClientPagination(containerId, pageSize) {
        pageSize = pageSize || 10;
        const container = document.getElementById(containerId);
        if (!container) return;

        // Support both table rows and .paginate-item divs
        const rows = Array.from(container.querySelectorAll('tbody tr:not(.no-paginate), .paginate-item'));
        if (rows.length <= pageSize) return; // No pagination needed

        let currentPage = 1;
        const totalPages = Math.ceil(rows.length / pageSize);

        function renderPage(page) {
            currentPage = page;
            rows.forEach((row, idx) => {
                const show = idx >= (page - 1) * pageSize && idx < page * pageSize;
                row.style.display = show ? '' : 'none';
            });
            renderPager();
        }

        function renderPager() {
            let existing = container.parentElement.querySelector('.admin-pager');
            if (existing) existing.remove();

            const pager = document.createElement('div');
            pager.className = 'admin-pager d-flex align-items-center justify-content-between mt-3 px-2';

            const info = document.createElement('span');
            info.className = 'text-muted small';
            const start = (currentPage - 1) * pageSize + 1;
            const end = Math.min(currentPage * pageSize, rows.length);
            info.textContent = `Hiển thị ${start}–${end} trong ${rows.length} mục`;
            pager.appendChild(info);

            const nav = document.createElement('nav');
            const ul = document.createElement('ul');
            ul.className = 'pagination pagination-sm mb-0';

            // Prev
            const prevLi = document.createElement('li');
            prevLi.className = 'page-item' + (currentPage === 1 ? ' disabled' : '');
            prevLi.innerHTML = `<a class="page-link rounded-start-3" href="#">«</a>`;
            prevLi.querySelector('a').addEventListener('click', e => { e.preventDefault(); if (currentPage > 1) renderPage(currentPage - 1); });
            ul.appendChild(prevLi);

            // Page numbers (show max 5 around current)
            let startP = Math.max(1, currentPage - 2);
            let endP = Math.min(totalPages, currentPage + 2);
            if (endP - startP < 4) {
                if (startP === 1) endP = Math.min(5, totalPages);
                else startP = Math.max(1, endP - 4);
            }

            for (let p = startP; p <= endP; p++) {
                const li = document.createElement('li');
                li.className = 'page-item' + (p === currentPage ? ' active' : '');
                li.innerHTML = `<a class="page-link" href="#">${p}</a>`;
                const pNum = p;
                li.querySelector('a').addEventListener('click', e => { e.preventDefault(); renderPage(pNum); });
                ul.appendChild(li);
            }

            // Next
            const nextLi = document.createElement('li');
            nextLi.className = 'page-item' + (currentPage === totalPages ? ' disabled' : '');
            nextLi.innerHTML = `<a class="page-link rounded-end-3" href="#">»</a>`;
            nextLi.querySelector('a').addEventListener('click', e => { e.preventDefault(); if (currentPage < totalPages) renderPage(currentPage + 1); });
            ul.appendChild(nextLi);

            nav.appendChild(ul);
            pager.appendChild(nav);
            container.parentElement.appendChild(pager);
        }

        renderPage(1);
    }

    window.initClientPagination = initClientPagination;

    /* =====================================================================
       ORDER STATUS UPDATE (AJAX)
    ===================================================================== */
    /**
     * initOrderStatusUpdate – intercept UpdateStatus form submit
     * @param {string} formId   id of the update-status form
     * @param {string} statusDisplayId  id of element showing current status text
     * @param {string} statusDotId      id of the status dot element
     */
    function initOrderStatusUpdate(formId, statusDisplayId, statusDotId) {
        const form = document.getElementById(formId);
        if (!form) return;

        form.addEventListener('submit', function (e) {
            e.preventDefault();

            const btn = form.querySelector('button[type="submit"], button.update-status-btn');
            if (btn) {
                btn.disabled = true;
                btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Đang lưu...';
            }

            const formData = new FormData(form);
            const body = new URLSearchParams(formData).toString();

            fetch(form.action || window.location.href, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded',
                    'X-Requested-With': 'XMLHttpRequest'
                },
                body: body
            })
            .then(r => r.json())
            .then(data => {
                if (data.success) {
                    showToast(data.message || 'Cập nhật trạng thái thành công!', 'success');
                    // Update status display text
                    if (statusDisplayId) {
                        const el = document.getElementById(statusDisplayId);
                        if (el && data.statusText) el.textContent = data.statusText;
                    }
                    // Update dot color
                    if (statusDotId && data.statusClass) {
                        const dot = document.getElementById(statusDotId);
                        if (dot) {
                            dot.className = 'status-dot ' + data.statusClass;
                        }
                    }
                    // Update mobile status card
                    const mobileStatus = document.getElementById('mobileStatusText');
                    if (mobileStatus && data.statusText) mobileStatus.textContent = data.statusText;
                    // Update the select to reflect saved state
                    const select = form.querySelector('select[name="status"]');
                    if (select) select.dataset.saved = select.value;
                } else {
                    showToast(data.message || 'Cập nhật thất bại!', 'danger');
                }
            })
            .catch(() => showToast('Lỗi kết nối, vui lòng thử lại!', 'danger'))
            .finally(() => {
                if (btn) {
                    btn.disabled = false;
                    btn.innerHTML = '<i class="bi bi-check-circle me-2"></i>Lưu thay đổi';
                }
            });
        });
    }

    window.initOrderStatusUpdate = initOrderStatusUpdate;

    /* =====================================================================
       MOBILE STICKY BUTTON for Order Status
    ===================================================================== */
    window.submitOrderStatusForm = function (formId) {
        const form = document.getElementById(formId);
        if (form) form.dispatchEvent(new Event('submit', { cancelable: true, bubbles: true }));
    };

})();
