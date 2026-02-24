document.addEventListener('DOMContentLoaded', function () {
    const deleteForm = document.getElementById('deleteForm');
    if (deleteForm) {
        deleteForm.addEventListener('submit', function (e) {
            const confirmMsg = deleteForm.dataset.confirm || 'Are you sure?';
            if (!confirm(confirmMsg)) {
                e.preventDefault();
            }
        });
    }
});
