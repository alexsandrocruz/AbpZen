$(function () {
    $(".PasswordVisibilityButton .bi-eye-slash").click(function (e) {
        let button = $(this);
        let passwordInput = button.parent().find("input");
        if (!passwordInput) {
            return;
        }

        if (passwordInput.attr("type") === "password") {
            passwordInput.attr("type", "text");
        }
        else {
            passwordInput.attr("type", "password");
        }

       button.toggleClass("bi-eye-slash").toggleClass("bi-eye");
    });
});
