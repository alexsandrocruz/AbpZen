(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var _identityUserAppService = volo.abp.identity.identityUser;
    
    $(function(){
        let getFormattedDate = function ($daterangesingle) {
            return moment($daterangesingle.val(), 'l', true).isValid() ? moment($daterangesingle.val(), 'l').format('YYYY-MM-DD') : null;
        };
        
        let getFilter = function (downloadToken) {
            let roleId = $("#IdentityUsersWrapper select#IdentityRole").val();
            let organizationUnitId = $("#IdentityUsersWrapper select#OrganizationUnit").val();

            return [
                { name: 'token', value: downloadToken },
                { name: 'filter', value: $('#IdentityUsersWrapper input.page-search-filter-text').val() },
                { name: 'roleId', value: !roleId ? null : roleId },
                { name: 'organizationUnitId', value: !organizationUnitId ? null : organizationUnitId },
                { name: 'userName', value: $("#IdentityUsersWrapper input#UserName").val() },
                { name: 'phoneNumber', value: $("#IdentityUsersWrapper input#PhoneNumber").val() },
                { name: 'emailAddress', value: $("#IdentityUsersWrapper input#EmailAddress").val() },
                { name: 'tenantName', value: $("#IdentityUsersWrapper input#TenantName").val() },
                { name: 'name', value: $("#IdentityUsersWrapper input#Name").val() },
                { name: 'surname', value: $("#IdentityUsersWrapper input#Surname").val() },
                { name: 'maxCreationTime', value: getFormattedDate($("#MaxCreationTime")) },
                { name: 'minCreationTime', value: getFormattedDate($("#MinCreationTime")) },
                { name: 'maxModifitionTime', value: getFormattedDate($("#MaxModificationTime")) },
                { name: 'minModifitionTime', value: getFormattedDate($("#MinModificationTime")) },
                { name: 'isLockedOut', value: $("#IdentityUsersWrapper select#IsLockedOut").val() },
                { name: 'notActive', value: $("#IdentityUsersWrapper select#NotActive").val() },
                { name: 'emailConfirmed', value: $("#IdentityUsersWrapper select#EmailConfirmed").val() },
                { name: 'isExternal', value: $("#IdentityUsersWrapper select#IsExternal").val() }
            ];
        };
        
        $('#ToExcelButton').click(function (e) {
            e.preventDefault();

            _identityUserAppService.getDownloadToken().then(
                function(result){
                    var url =  abp.appPath + 'api/identity/users/export-as-excel' +
                        abp.utils.buildQueryString(getFilter(result.token));

                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
                }
            )
        });

        $('#ToCsvButton').click(function (e) {
            e.preventDefault();

            _identityUserAppService.getDownloadToken().then(
                function(result){
                    var url =  abp.appPath + 'api/identity/users/export-as-csv' +
                        abp.utils.buildQueryString(getFilter(result.token));

                    var downloadWindow = window.open(url, '_blank');
                    downloadWindow.focus();
                }
            )
        });
    })
})(jQuery);