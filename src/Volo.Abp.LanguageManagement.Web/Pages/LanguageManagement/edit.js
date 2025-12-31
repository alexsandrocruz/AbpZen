var abp = abp || {};
$(function () {
    abp.modals.languageEdit = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();

            $form.find(".select2-container").addClass("d-block");
        };

        return {
            initModal: initModal
        };
    };
});