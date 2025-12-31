var abp = abp || {};
$(function () {
    abp.modals.languageCreate = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();

            $('#Language_CultureName').change(function () {
                $("#Language_UiCultureName").val($('#Language_CultureName').val());
                $("#Language_DisplayName").val($('#Language_CultureName option:selected').text());
            });

            $form.find(".select2-container").addClass("d-block");
        };

        return {
            initModal: initModal
        };
    };
});