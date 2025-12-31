var abp = abp || {};

abp.modals.newsletterSuccessModal = function () {
    var initModal = function (modalManager, args) {
        var $modal = modalManager.getModal();
        var $form = modalManager.getForm();

        if(!$form) {
            $modal.find("#newsletterSuccessBtn").on("click", function () {
                abp.event.trigger('newsletterEmailSubmitted');
                modalManager.close();
            });
        } else {
            $form.on("submit", function (e) {
                e.preventDefault();

                abp.ui.setBusy($modal.find("#newsletterSuccessBtn"));

                var additionalPreferences = [];
                var selectedPreferences = $('#additional-preferences').find('.additional-checkbox:checked');

                for (var i = 0; i < selectedPreferences.length; i++) {
                    additionalPreferences[i] = $(selectedPreferences[i]).attr('data-additional-preference');
                }
                
                var payload = {
                    emailAddress: $form.attr('data-email'),
                    preference: $form.attr('data-preference'),
                    source: $form.attr('data-source'),
                    sourceUrl: $form.attr('data-sourceurl'),
                    additionalPreferences: additionalPreferences
                };
                
                volo.cmsKit.public.newsletters.newsletterRecordPublic.create(payload)
                    .then(function () {
                        abp.event.trigger('newsletterEmailSubmitted');
                        modalManager.close();
                        abp.ui.clearBusy();
                    })
                    .catch(function () {
                        abp.ui.clearBusy();
                    });
            });
        }
    };

    return {
        initModal: initModal
    };
};