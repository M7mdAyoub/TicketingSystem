document.addEventListener('DOMContentLoaded', function () {
    const openBtn = document.getElementById('openCategoryModalBtn');
    if (openBtn) {
        openBtn.addEventListener('click', openCategoryModal);
    }

    const closeBtns = document.querySelectorAll('.close-category-modal-btn');
    closeBtns.forEach(btn => btn.addEventListener('click', closeCategoryModal));

    const overlayTriggers = document.querySelectorAll('.modal-overlay-trigger');
    overlayTriggers.forEach(overlay => {
        overlay.addEventListener('click', function (e) {
            if (e.target === this) {
                if (this.id === 'categoryModal') closeCategoryModal();
            }
        });
    });

    var catForm = document.getElementById('catForm');
    if (catForm) {
        catForm.addEventListener('submit', function (e) {
            e.preventDefault();
            var btn = document.getElementById('catSubmitBtn');
            btn.disabled = true;

            var originalBtnHtml = btn.innerHTML;
            btn.innerHTML = '<i class="bi bi-hourglass-split me-1"></i> ...';
            document.getElementById('catModalError').style.display = 'none';

            var formData = new FormData();
            formData.append('name', document.getElementById('catName').value);
            formData.append('isActive', document.getElementById('catIsActive').checked);
            formData.append('__RequestVerificationToken', catForm.dataset.token);

            fetch(catForm.dataset.url, { method: 'POST', body: formData })
                .then(r => r.json())
                .then(data => {
                    if (data.success) {
                        var emptyRow = document.getElementById('emptyRow');
                        if (emptyRow) emptyRow.remove();

                        var tbody = document.getElementById('categoriesBody');
                        var tr = document.createElement('tr');
                        tr.className = 'fade-in-row';

                        var badgeClass = data.isActive ? 'badge-active' : 'badge-inactive';
                        var badgeText = data.isActive ? catForm.dataset.txtActive : catForm.dataset.txtInactive;

                        tr.innerHTML = `
                            <td>
                                <div class="d-flex align-items-center gap-3">
                                    <div class="category-icon-bg">
                                        <i class="bi bi-tag-fill"></i>
                                    </div>
                                    <div>
                                        <div class="category-name-text">${data.name}</div>
                                        <div class="category-id-text">ID: ${data.id}</div>
                                    </div>
                                </div>
                            </td>
                            <td><span class="badge-status ${badgeClass}">${badgeText}</span></td>
                            <td class="category-date-text">${data.createdDate}</td>
                            <td><span class="category-action-placeholder">↻</span></td>
                        `;
                        tbody.appendChild(tr);
                        closeCategoryModal();
                    } else {
                        document.getElementById('catModalError').textContent = data.error;
                        document.getElementById('catModalError').style.display = 'block';
                    }
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                })
                .catch(() => {
                    document.getElementById('catModalError').textContent = 'Network error.';
                    document.getElementById('catModalError').style.display = 'block';
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                });
        });

        document.addEventListener('keydown', function (e) {
            if (e.key === 'Escape') closeCategoryModal();
        });
    }
});

function openCategoryModal() {
    var modal = document.getElementById('categoryModal');
    if (modal) {
        modal.className = "modal-overlay modal-active";
        document.getElementById('catName').value = '';
        document.getElementById('catIsActive').checked = true;
        document.getElementById('catModalError').style.display = 'none';
        setTimeout(function () { document.getElementById('catName').focus(); }, 100);
    }
}

function closeCategoryModal() {
    var modal = document.getElementById('categoryModal');
    if (modal) modal.className = "modal-overlay";
}