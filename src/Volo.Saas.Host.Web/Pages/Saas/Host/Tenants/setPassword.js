
$(function () {
    abp.modals.changeTenantPassword = function () {
        let initModal = function (publicApi, args) {
            let $newPasswordInput = $("#NewPasswordInput");
            let generateRandomPasswordButton = $("#GenerateRandomPasswordButton");
            let passwordVisibilityButton = $("#PasswordVisibilityButton");

            let specials = '!*_#/+-.';
            let lowercase = 'abcdefghjkmnpqrstuvwxyz';
            let uppercase = lowercase.toUpperCase();
            let numbers = '23456789';

            let all = specials + lowercase + uppercase + numbers;

            generateRandomPasswordButton.click(function () {
                var password = '';
                password += pickLetters(specials, 1);
                password += pickLetters(lowercase, 1);
                password += pickLetters(uppercase, 1);
                password += pickLetters(numbers, 1);
                password += pickLetters(all, 4, 4);

                password = shuffleString(password);

                $newPasswordInput.val(password);
                $newPasswordInput.attr("type", "text");

                let icon = $(this).find("i");
                if (icon) {
                    passwordVisibilityButton.find("i").removeClass("fa-eye-slash").addClass("fa-eye");
                }
            });

            passwordVisibilityButton.click(function(){
                let passwordInput = $(this).parent().find("input");
                if (!passwordInput) {
                    return;
                }

                if (passwordInput.attr("type") === "password") {
                    passwordInput.attr("type", "text");
                }
                else {
                    passwordInput.attr("type", "password");
                }

                let icon = $(this).find("i");
                if (icon) {
                    icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
                }
            });

            function pickLetters(text, min, max) {
                var n, chars = '';

                if (typeof max === 'undefined') {
                    n = min;
                } else {
                    n = min + parseInt(Math.random() * (max - min + 1));
                }
                for (var i = 0; i < n; i++) {
                    chars += text.charAt(parseInt(Math.random() * text.length));
                }

                return chars;
            }

            function shuffleString(string) {
                var parts = string.split('');
                for (var i = parts.length; i > 0;) {
                    var random = parseInt(Math.random() * i);
                    var temp = parts[--i];
                    parts[i] = parts[random];
                    parts[random] = temp;
                }
                return parts.join('');
            }
        };
        return {
            initModal: initModal
        };
    };
});
