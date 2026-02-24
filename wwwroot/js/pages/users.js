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

                        var statusHtml = data.isActive
                            ? `<div style="${abstractStyle}">
                                 <span class="user-badge-active-span">
                                   <span class="user-badge-text-active">${userForm.dataset.txtActive}</span>
                                   <div class="user-badge-dot-container-active">
                                     <div class="user-badge-dot-inner-active"></div>
                                   </div>
                                 </span>
                               </div>`
                            : `<div style="${abstractStyle}">
                                 <span class="user-badge-inactive-span">
                                   <span class="user-badge-text-inactive">${userForm.dataset.txtInactive}</span>
                                 </span>
                               </div>`;

                        var onlineDot = data.isActive
                            ? '<div class="user-avatar-online-dot"></div>'
                            : '';

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
                                    <div class="user-action-btn-primary" style="cursor: pointer;" onclick="openEmailUserModal('${data.email}')" title="${userForm.dataset.txtEmail}">
                                        <i class="bi bi-envelope"></i>
                                    </div>
                                    <div class="user-action-btn-secondary" style="cursor: pointer;" onclick="openEditUserModal('${data.id}', '${data.fullName.replace(/'/g, "\\'")}', '${data.email}', ${data.isActive})" title="${userForm.dataset.txtMore}">
                                        <i class="bi bi-three-dots-vertical"></i>
                                    </div>
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
                    document.getElementById('userModalError').textContent = 'Network error.';
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
                    document.getElementById('editUserModalError').textContent = 'Network error.';
                    document.getElementById('editUserModalError').style.display = 'block';
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                });
        });
    }

    var emailUserForm = document.getElementById('emailUserForm');
    if (emailUserForm) {
        emailUserForm.addEventListener('submit', function (e) {
            e.preventDefault();
            var btn = document.getElementById('emailUserSubmitBtn');
            btn.disabled = true;

            var originalBtnHtml = btn.innerHTML;
            btn.innerHTML = '<i class="bi bi-hourglass-split me-1"></i> ...';
            document.getElementById('emailUserModalError').style.display = 'none';
            document.getElementById('emailUserModalSuccess').classList.add('d-none');

            var formData = new FormData();
            formData.append('email', document.getElementById('emailUserAddress').value);
            formData.append('title', document.getElementById('emailUserTitle').value);
            formData.append('subject', document.getElementById('emailUserSubject').value);
            var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
            if (tokenInput) formData.append('__RequestVerificationToken', tokenInput.value);

            fetch(emailUserForm.dataset.url, { method: 'POST', body: formData })
                .then(r => r.json())
                .then(data => {
                    if (data.success) {
                        document.getElementById('emailUserModalSuccess').classList.remove('d-none');
                        document.getElementById('emailUserTitle').value = '';
                        document.getElementById('emailUserSubject').value = '';
                        setTimeout(() => closeEmailUserModal(), 2000);
                    } else {
                        document.getElementById('emailUserModalError').textContent = data.error;
                        document.getElementById('emailUserModalError').style.display = 'block';
                    }
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                })
                .catch(() => {
                    document.getElementById('emailUserModalError').textContent = 'Network error.';
                    document.getElementById('emailUserModalError').style.display = 'block';
                    btn.disabled = false;
                    btn.innerHTML = originalBtnHtml;
                });
        });
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            closeUserModal();
            closeEditUserModal();
            closeEmailUserModal();
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

function openEmailUserModal(email) {
    var modal = document.getElementById('emailUserModal');
    if (modal) {
        modal.className = "modal-overlay modal-active";
        document.getElementById('emailUserAddress').value = email;
        document.getElementById('emailUserAddressDisplay').value = email;
        document.getElementById('emailUserTitle').value = '';
        document.getElementById('emailUserSubject').value = '';
        document.getElementById('emailUserModalError').style.display = 'none';
        document.getElementById('emailUserModalSuccess').classList.add('d-none');
        setTimeout(function () { document.getElementById('emailUserTitle').focus(); }, 100);
    }
}

function closeEmailUserModal() {
    var modal = document.getElementById('emailUserModal');
    if (modal) modal.className = "modal-overlay";
}
