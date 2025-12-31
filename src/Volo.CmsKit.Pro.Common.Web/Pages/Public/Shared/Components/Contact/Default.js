(function ($) {
    var l = abp.localization.getResource("CmsKit");

    abp.widgets.CmsContact = function ($widget) {
        var widgetManager = $widget.data("abp-widget-manager");
        var $contactArea = $widget.find(".cms-contact-area");
        var contactName = $contactArea.attr("data-widget");

        function getFilters() {
            return {
                contactName: contactName,
            };
        }

        function init() {
            $widget.find(".contact-form").on('submit', '', function (e) {
                e.preventDefault();

                var formAsObject = $(this).serializeFormToObject();

                volo.cmsKit.public.contact.contactPublic.sendMessage(
                    {
                        contactName: contactName,
                        name: formAsObject.name,
                        subject: formAsObject.subject,
                        email: formAsObject.emailAddress,
                        message: formAsObject.message,
                        recaptchaToken: formAsObject.recaptchaToken
                    }
                ).then(function () {
                    abp.message.success(l("ContactSuccess"))
                    widgetManager.refresh($widget);
                })
            });
        }

        return {
            init: init,
            getFilters: getFilters
        }
    };
})(jQuery);
