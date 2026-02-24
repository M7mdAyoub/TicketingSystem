document.addEventListener('DOMContentLoaded', function () {
    const hamburgerBtn = document.querySelector('.hamburger-btn');
    const mobileNavOverlay = document.getElementById('mobileNavOverlay');
    const mobileNavDrawer = document.getElementById('mobileNavDrawer');
    const hamburgerCloseBtn = document.querySelector('.hamburger-close-btn');

    if (hamburgerBtn) {
        hamburgerBtn.addEventListener('click', toggleMobileNav);
    }

    if (mobileNavOverlay) {
        mobileNavOverlay.addEventListener('click', closeMobileNav);
    }

    if (hamburgerCloseBtn) {
        hamburgerCloseBtn.addEventListener('click', closeMobileNav);
    }

    function toggleMobileNav() {
        if (mobileNavOverlay) mobileNavOverlay.classList.toggle('active');
        if (mobileNavDrawer) mobileNavDrawer.classList.toggle('active');
        document.body.style.overflow = (mobileNavDrawer && mobileNavDrawer.classList.contains('active')) ? 'hidden' : '';
    }

    function closeMobileNav() {
        if (mobileNavOverlay) mobileNavOverlay.classList.remove('active');
        if (mobileNavDrawer) mobileNavDrawer.classList.remove('active');
        document.body.style.overflow = '';
    }
});
