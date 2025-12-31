(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var _identityUserAppService = volo.abp.identity.identityUser;

    
    abp.modals.ImportUserFromFileModal = function () {
        var initModal = function (publicApi, args) {

            var UPPY_UPLOAD_ENDPOINT = $("#uploadEndpoint").val();
            var fileType = $("#fileType").val();
            var _publicApi = publicApi;
            
            document.querySelector("#importUserFile").onchange = function() {
                var span = $("span[name=fileName]");
                if(this.files.length === 0){
                    span.text(l("ChooseFile")); 
                    return;
                }
                
                var fileName = this.files[0]?.name;
                span.text(fileName ?? span.text());
            };
            
            $("#downloadSampleImportFile").click(function(){
                _identityUserAppService.getDownloadToken().then(
                    function(result){
                        var url =  abp.appPath + 'api/identity/users/import-users-sample-file' +
                            abp.utils.buildQueryString([
                                { name: 'token', value: result.token },
                                { name: 'fileType', value: fileType },
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

            var fileInput = $("#importUserFile");

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
                    data: fileInput[0].files[0],
                    meta:{
                        fileType: fileType,
                    }
                });

                UPPY.upload().then((response) => {
                    $this.buttonBusy(false);
                    
                    if (response.failed.length > 0) {
                        var result = response.failed[0].response.body;
                        abp.message.error(result.error.message);
                    } else {
                        var result = response.successful[0].response.body;
                        
                        if(result.isAllSucceeded){
                            abp.message.success(l("ImportSuccessMessage")).done(function(){
                                _publicApi.close();
                            });
                        }else{
                            var token = result.invalidUsersDownloadToken;
                            var url = abp.appPath + 'api/identity/users/download-import-invalid-users-file?token=' + token;

                            abp.message.confirm(l("ImportFailedMessage", result.succeededCount, result.failedCount)).then(function(confirmed){
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
