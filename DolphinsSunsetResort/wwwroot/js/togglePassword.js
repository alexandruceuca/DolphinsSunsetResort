$(document).ready(function () {
    document.querySelectorAll('.toggle-password').forEach(button => {
        button.addEventListener('click', function () {
            const targetField = document.getElementById(this.getAttribute('data-target'));
            const type = targetField.getAttribute('type') === 'password' ? 'text' : 'password';
            targetField.setAttribute('type', type);

            // Toggle button icon
            this.textContent = type === 'password' ? '👁️' : '🙈';
        });
    });
});