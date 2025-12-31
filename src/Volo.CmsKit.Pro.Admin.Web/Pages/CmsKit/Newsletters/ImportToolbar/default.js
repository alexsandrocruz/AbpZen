(function ($) {
    
    var _importNewsletterFromFileModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'CmsKit/Newsletters/ImportToolbar/ImportNewslettersFromFileModal',
        scriptUrl: abp.appPath +'Pages/CmsKit/Newsletters/ImportToolbar/importNewslettersFromFileModal.js',
        modalClass: "ImportNewslettersFromFileModal"
    });

    var _dataTable = null;

    $(function(){

        _dataTable = $('#NewslettersTable').data('dataTable');

        $('#FromCsvButton').click(function (e) {
            e.preventDefault();
            _importNewsletterFromFileModal.open();
        });

        _importNewsletterFromFileModal.onClose(function () {
            _dataTable.ajax.reloadEx();
        });
    })
})(jQuery);