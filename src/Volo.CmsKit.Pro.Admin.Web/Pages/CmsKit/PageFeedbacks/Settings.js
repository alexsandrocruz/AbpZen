var abp = abp || {};
$(function () {

    var l = abp.localization.getResource("CmsKit");

    abp.modals.settingsPageFeedback = function () {

        var initModal = function (modalManager) {
            var $form = modalManager.getForm();
            $form.validate({
                errorElement: 'span',
                errorClass: "text-danger d-block small",
            });
            $.each($form.find("[data-val-multiple-emails]"), function (index, element) {
                var message = $(element).data("val-multiple-emails-message");
                $(element).rules("add", {
                    multipleEmails: true,
                    messages: {
                        multipleEmails: message
                    }
                });
            });

            $.validator.addMethod("multipleEmails", function (value, element) {
                if (this.optional(element)) {
                    return true;
                }

                var emailsArray = value.split($(element).data("val-multiple-emails-delimiter"));

                for (var i = 0; i < emailsArray.length; i++) {
                    var email = emailsArray[i].trim();

                    if (!isValidEmail(email)) {
                        return false;
                    }
                }

                return true;
            });

            function isValidEmail(value) {
                if (value.trim() === "") {
                    return false;
                }

                var index = value.indexOf('@');

                return index > 0 && index !== value.length - 1 && index === value.lastIndexOf('@');
            }

        };
        return {
            initModal: initModal
        };
    }
});