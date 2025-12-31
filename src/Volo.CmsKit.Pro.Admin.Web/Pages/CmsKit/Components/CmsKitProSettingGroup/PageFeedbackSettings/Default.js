$(function () {
    $("#CmsKitProPageFeedbackSettingsForm").on("submit", function (e) {
        e.preventDefault();
        
        var form = $(this).serializeFormToObject();
        
        volo.cmsKit.admin.pageFeedbacks.pageFeedbackSettings.update(form).then(function (result) {
            $(document).trigger("AbpSettingSaved");
        });
    });
});