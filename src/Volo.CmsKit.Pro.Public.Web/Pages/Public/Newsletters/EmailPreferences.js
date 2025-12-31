(function () {
    var l = abp.localization.getResource("CmsKit");

    var newsletterService = volo.cmsKit.public.newsletters.newsletterRecordPublic;

    var preferenceSelectors = document.querySelectorAll('.newsletter-preference-check');
    
    var additionalPreferenceSelector = $("#additional-preferences");

    var subscribeForm = $('#newsletterSubscribe');
    
    var subscribeButton = $('#subscribeNewsletterButton');
    
    $('form#newsletterSubscribe').submit(function (e) {
        e.preventDefault();

        var source = $("#newsletterSubscribe").data("source");
        var sourceUrl = window.location.href;
        var $newsletterManage = $('#newsletter-manage');
        var emailAddress = $newsletterManage.data('emailaddress');
        var preferenceDetails = [];
        
        preferenceSelectors.forEach(function (value) {
            preferenceDetails.push({preference: value.getAttribute('data-preference'), isEnabled: value.checked});
        });

        var additionalPreferences = [];
        if (additionalPreferenceSelector.length) {
            additionalPreferenceSelector.find('input:checked').each(function () {
                additionalPreferences.push($(this).data('preference'));
            });
        }

        var enabledPreferences = preferenceDetails.filter(function(item) {
            return item.isEnabled === true;
        });
        
        if(enabledPreferences.length > 0){
            if(enabledPreferences.length > 1){
                additionalPreferences = enabledPreferences.slice(1).map(function(preference) {
                    return preference.preference;
                });
            }
            newsletterService.create({
                emailAddress: emailAddress,
                source: source,
                sourceUrl: sourceUrl,
                preferenceDetails: preferenceDetails,
                additionalPreferences: additionalPreferences,
                preference: enabledPreferences[0].preference
            }).then(function (r) {
                abp.message.success(l("NewsletterSuccessMessage")).then(function () {
                    window.location.reload();
                });
            });
        }
    });

    subscribeForm.on('change submit', function () {
        checkAndUpdateButton();
    });

    function checkAndUpdateButton() {
        var checkboxList = subscribeForm.find('.newsletter-preference-check');
        var atLeastOneChecked = checkboxList.is(':checked');

        subscribeButton.prop('disabled', !atLeastOneChecked);
    }
    
    $('#selectAll').click(function (event) {
        if (this.checked) {
            $('.newsletter-preference-check').each(function () {
                $(this).prop('checked', true);
            });
        } else {
            $('.newsletter-preference-check').each(function () {
                $(this).prop('checked', false);
            });
        }
    });

    $(window).on("load", "", function () {
        if ($('.newsletter-preference-check:checked').length === $('.newsletter-preference-check').length) {
            $('#selectAll').prop('checked', true);
        } else {
            $('#selectAll').prop('checked', false);
        }
    });

    $(".newsletter-preference-check").on("change", "", function () {
        if ($('.newsletter-preference-check:checked').length === $('.newsletter-preference-check').length) {
            $('#selectAll').prop('checked', true);
        } else {
            $('#selectAll').prop('checked', false);
        }
    });

    $('form#newsletterUpdate').submit(function (e) {
        e.preventDefault();
        var sourceUrl = window.location.href;
        var $newsletterManage = $('#newsletter-manage');
        var emailAddress = $newsletterManage.data('emailaddress');
        var securityCode = $newsletterManage.data('securitycode');
        var source = $newsletterManage.data('source');

        var preferenceDetails = [];
        preferenceSelectors.forEach(function (value) {
            preferenceDetails.push({preference: value.getAttribute('data-preference'), isEnabled: value.checked});
        });

        newsletterService.updatePreferences(
            {
                emailAddress: emailAddress,
                preferenceDetails: preferenceDetails,
                source: source,
                sourceUrl: sourceUrl,
                securityCode: securityCode
            })
            .then(function (r) {
                abp.message.success(l("UpdatePreferenceSuccessMessage"))
                    .then(function () {
                        window.location.reload();
                    });
            });
    });
})(jQuery);
