document.addEventListener('DOMContentLoaded', function () {
    var userForm = document.getElementById('userForm');
    if (userForm) {
        userForm.addEventListener('submit', function (e) {
            e.preventDefault();
            var btn = document.getElementById('userSubmitBtn');
            btn.disabled = true;

            var originalBtnHtml = btn.innerHTML;
            btn.innerHTML = '<i class="bi bi-hourglass-split me-1"></i> ...';
            document.getElementById('userModalError').style.display = 'none';

            var formData = new FormData();
            formData.append('fullName', document.getElementById('userFullName').value);
            formData.append('email', document.getElementById('userEmail').value);
            formData.append('password', document.getElementById('userPassword').value);
            formData.append('isActive', document.getElementById('userIsActive').checked);
            formData.append('__RequestVerificationToken', userForm.dataset.token);

            fetch(userForm.dataset.url, { method: 'POST', body: formData })
                .then(r => r.json())
                .then(data => {
                    if (data.success) {
                        var emptyDiv = document.getElementById('emptyUsers');
                        if (emptyDiv) emptyDiv.remove();

                        var grid = document.getElementById('usersGrid');
                        var col = document.createElement('div');
                        col.className = 'col-md-6 col-lg-3 fade-in-row';

                        var statusSide = userForm.dataset.lang === 'ar' ? 'left' : 'right';
                        var abstractStyle = `position:absolute;top:12px;${statusSide}:12px;`;

                        var statusHtml = `
                            <div style="${abstractStyle}">
                                <div class="user-action-btn-secondary" style="cursor: pointer;" onclick="openEditUserModal('${data.id}', '${data.fullName.replace(/'/g, "\\'")}', '${data.email}', ${data.isActive})" title="${userForm.dataset.txtEdit}">
                                    <i class="bi bi-three-dots"></i>
                                </div>
                            </div>
                        `;

                        var onlineDot = data.isActive
                            ? '<div class="user-avatar-online-dot"></div>'
                            : '';

                        var bottomAction = data.isActive
                            ? `<button class="user-badge-active-span border-0" type="submit" title="${userForm.dataset.txtActive}">
                                 <span class="user-badge-text-active">${userForm.dataset.txtActive}</span>
                                 <div class="user-badge-dot-container-active">
                                   <div class="user-badge-dot-inner-active"></div>
                                 </div>
                               </button>`
                            : `<button class="user-badge-inactive-span border-0" type="submit" title="${userForm.dataset.txtInactive}">
                                 <span class="user-badge-text-inactive">${userForm.dataset.txtInactive}</span>
                               </button>`;

                        col.innerHTML = `
                            <div class="card user-card-wrap">
                                ${statusHtml}
                                <div class="user-avatar-wrap">
                                    <div class="user-avatar-circle">
                                        ${data.initials}
                                    </div>
                                    ${onlineDot}
                                </div>
                                <h5 class="user-card-name">${data.fullName}</h5>
                                <div class="user-card-team">${userForm.dataset.txtTeam}</div>
                                <div class="user-card-email">${data.email}</div>
                                <div class="d-flex justify-content-center gap-2">
                                    <form action="/Users/ToggleActive" method="post" style="margin:0;">
                                        <input type="hidden" name="__RequestVerificationToken" value="${userForm.dataset.token}" />
                                        <input type="hidden" name="id" value="${data.id}" />
                                        ${bottomAction}
                                    </form>
                                </div>
                            </div>
                        `;
                        grid.appendChild(col);
                        closeUserModal();
                    } else {
                        document.getElementById('userModalError').textContent = data.error;
                        document.getElementById('userModalError').style.display = 'block';
                    }
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                })
                .catch(() => {
                    document.getElementById('userModalError').textContent = userForm.dataset.txtNetworkError || 'Network error.';
                    document.getElementById('userModalError').style.display = 'block';
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                });
        });
    }

    var editUserForm = document.getElementById('editUserForm');
    if (editUserForm) {
        editUserForm.addEventListener('submit', function (e) {
            e.preventDefault();
            var btn = document.getElementById('editUserSubmitBtn');
            btn.disabled = true;

            var originalBtnHtml = btn.innerHTML;
            btn.innerHTML = '<i class="bi bi-hourglass-split me-1"></i> ...';
            document.getElementById('editUserModalError').style.display = 'none';

            var formData = new FormData();
            formData.append('id', document.getElementById('editUserId').value);
            formData.append('fullName', document.getElementById('editUserFullName').value);
            formData.append('email', document.getElementById('editUserEmail').value);
            formData.append('isActive', document.getElementById('editUserIsActive').checked);
            var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            if (tokenInput) formData.append('__RequestVerificationToken', tokenInput.value);

            fetch(editUserForm.dataset.url, { method: 'POST', body: formData })
                .then(r => r.json())
                .then(data => {
                    if (data.success) {
                        window.location.reload();
                    } else {
                        document.getElementById('editUserModalError').textContent = data.error;
                        document.getElementById('editUserModalError').style.display = 'block';
                    }
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                })
                .catch(() => {
                    document.getElementById('editUserModalError').textContent = editUserForm.dataset.txtNetworkError || 'Network error.';
                    document.getElementById('editUserModalError').style.display = 'block';
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                });
        });
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            closeUserModal();
            closeEditUserModal();
        }
    });
});

function openUserModal() {
    var modal = document.getElementById('userModal');
    if (modal) {
        modal.className = "modal-overlay modal-active";
        document.getElementById('userFullName').value = '';
        document.getElementById('userEmail').value = '';
        document.getElementById('userPassword').value = '';
        document.getElementById('userIsActive').checked = true;
        document.getElementById('userModalError').style.display = 'none';
        setTimeout(function () { document.getElementById('userFullName').focus(); }, 100);
    }
}

function closeUserModal() {
    var modal = document.getElementById('userModal');
    if (modal) modal.className = "modal-overlay";
}

function openEditUserModal(id, fullname, email, isActive) {
    var modal = document.getElementById('editUserModal');
    if (modal) {
        modal.className = "modal-overlay modal-active";
        document.getElementById('editUserId').value = id;
        document.getElementById('editUserFullName').value = fullname;
        document.getElementById('editUserEmail').value = email;
        document.getElementById('editUserIsActive').checked = isActive;
        document.getElementById('editUserModalError').style.display = 'none';
        setTimeout(function () { document.getElementById('editUserFullName').focus(); }, 100);
    }
}

function closeEditUserModal() {
    var modal = document.getElementById('editUserModal');
    if (modal) modal.className = "modal-overlay";
}

