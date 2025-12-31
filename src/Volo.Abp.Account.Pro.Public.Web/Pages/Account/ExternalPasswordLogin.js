$(function () {
    $("#PasswordVisibilityButton").click(function (e) {
        let button = $(this);
        let passwordInput = $('#password-input');
        if (!passwordInput) {
            return;
        }

        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text");
        }
        else {
            passwordInput.attr("type", "password");
        }

        let icon = $("#PasswordVisibilityButton");
        if (icon) {
            icon.toggleClass("bi-eye-slash").toggleClass("bi-eye");
        }
    });
});
