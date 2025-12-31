(function ($) {
    var additionalPreferences = [];

    var successModal = new abp.ModalManager({
        viewUrl: abp.appPath + "Public/Newsletters/SuccessModal",
        scriptUrl: abp.appPath + "Pages/Public/Newsletters/successModal.js",
        modalClass: "newsletterSuccessModal"
    });
    
    abp.widgets.CmsNewsletter = function ($widget) {
        var $newsletterArea = $widget.find(".cms-newsletter-area");
        var $additionalPreferenceSelector = $widget.find('#additional-preference');
        var $form = $widget.find("form")

        var dataPreferenceValue = $newsletterArea.attr("data-preference");
        var dataSourceValue = $newsletterArea.attr("data-source");
        var normalizedDataSource = dataSourceValue.replace('.', '_');
        
        function getFilters() {
            return {
                preference: dataPreferenceValue,
                source: dataSourceValue,
            };
        }

        function init() {
            $form.on('submit', function (e) {
                e.preventDefault();

                abp.ui.setBusy($form.find("button[type='submit']"));
                
                var formAsObject = $(this).serializeFormToObject();
                if (formAsObject === undefined || !formAsObject.newsletterEmail) {
                    formAsObject = {
                        newsletterEmail: $('#newsletter-email-input-' + normalizedDataSource).val()
                    }
                }

                if ($additionalPreferenceSelector.length) {
                    for (var i = 0; i < $additionalPreferenceSelector[0].children.length; i++) {
                        var additionalPreferenceCheckBox = "#additional-" + $additionalPreferenceSelector[0].children[i].attributes[1].value + "-" + normalizedDataSource;
                        if ($(additionalPreferenceCheckBox).is(":checked")) {
                            additionalPreferences.push($additionalPreferenceSelector[0].children[i].attributes[1].value);
                        }
                    }
                }
                      
                var preference = dataPreferenceValue;
                var sourceUrl = window.location.href;

                var createPayload = {
                    emailAddress: formAsObject.newsletterEmail,
                    preference: preference,
                    source: dataSourceValue,
                    sourceUrl: sourceUrl,
                    additionalPreferences: additionalPreferences
                };

                volo.cmsKit.public.newsletters.newsletterRecordPublic.create(createPayload)
                    .then(function () {
                        abp.ui.clearBusy();

                        successModal.open({
                            emailAddress: formAsObject.newsletterEmail,
                            preference: preference,
                            source: dataSourceValue,
                            sourceUrl: sourceUrl,
                            requestAdditionalPreferences: $newsletterArea.attr('data-get-preferences-later')
                        });
                    })
                    .catch(function () {
                        abp.ui.clearBusy();
                    });
            });
        }

        return {
            init: init,
            getFilters: getFilters
        }
    };

})(jQuery);
