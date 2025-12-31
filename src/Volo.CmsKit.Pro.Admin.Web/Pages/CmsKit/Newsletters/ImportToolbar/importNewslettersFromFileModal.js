(function ($) {

    var l = abp.localization.getResource('CmsKit');
    var _newslettersAppService = volo.cmsKit.admin.newsletters.newsletterRecordAdmin;


    abp.modals.ImportNewslettersFromFileModal = function () {
        var initModal = function (publicApi, args) {

            var UPPY_UPLOAD_ENDPOINT = $("#uploadEndpoint").val();
            var _publicApi = publicApi;

            document.querySelector("#importNewsletterFile").onchange = function() {
                var span = $("span[name=fileName]");
                if(this.files.length === 0){
                    span.text(l("ChooseFile"));
                    return;
                }

                var fileName = this.files[0]?.name;
                span.text(fileName ?? span.text());
            };

            $("#downloadSampleImportFile").click(function(){
                _newslettersAppService.getDownloadToken().then(
                    function(result){
                        var url =  abp.appPath + 'api/cms-kit-admin/newsletter/import-newsletters-sample-file' +
                            abp.utils.buildQueryString([
                                { name: 'token', value: result.token },
                            ]);

                        var downloadWindow = window.open(url, '_blank');
                        downloadWindow.focus();
                    }
                )
            });

            function getUppyHeaders() {
                var headers = {};
                headers[abp.security.antiForgery.tokenHeaderName] = abp.security.antiForgery.getToken();

                return headers;
            }

            var UPPY_OPTIONS = {
                endpoint: UPPY_UPLOAD_ENDPOINT,
                formData: true,
                fieldName: "File",
                method: "post",
                headers: getUppyHeaders(),
            };

            var UPPY = new Uppy.Uppy().use(Uppy.XHRUpload, UPPY_OPTIONS);
            var UPPY_FILE_ID = "uppy-upload-file";

            var fileInput = $("#importNewsletterFile");

            $("#uploadImportFileBtn").click(function (e) {
                e.preventDefault();

                if(fileInput.val() === ""){
                    abp.message.warn(l("PleaseSelectAFile"));
                    return;
                }

                var $this = $(this);
                $this.buttonBusy();

                UPPY.cancelAll();

                UPPY.addFile({
                    id: UPPY_FILE_ID,
                    name: fileInput[0].files[0].name,
                    data: fileInput[0].files[0]
                });

                UPPY.upload().then((response) => {
                    $this.buttonBusy(false);

                    if (response.failed.length > 0) {
                        var result = response.failed[0].response.body;
                        abp.message.error(result.error.message);
                    } else {
                        var result = response.successful[0].response.body;

                        if(result.isAllSucceeded){
                            abp.message.success(l("NewsletterImportSuccessMessage")).done(function(){
                                _publicApi.close();
                            });
                        }else{
                            var token = result.invalidNewslettersDownloadToken;
                            var url = abp.appPath + 'api/cms-kit-admin/newsletter/download-import-invalid-newsletters-file?token=' + token;

                            abp.message.confirm(l("NewsletterImportFailedMessage", result.succeededCount, result.failedCount)).then(function(confirmed){
                                if(confirmed){
                                    var downloadWindow = window.open(url, '_blank');
                                    downloadWindow.focus();
                                }
                            });
                        }
                    }
                });
            });
        };

        return {
            initModal: initModal
        };
    };

})(jQuery);