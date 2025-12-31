(function () {
    var l = abp.localization.getResource("AbpAccount");

    var _identityLinkUser = volo.abp.account.identityLinkUser;

    _dataTable = $("#MyLinkUsersTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
            aLengthMenu: [ 5, 10, 25, 50, 100 ],
            order: [],
            ajax: abp.libs.datatables.createAjax(_identityLinkUser.getAllList),
            columnDefs: abp.ui.extensions.tableColumns
                .get("account.linkUsers")
                .columns.toArray()
        })
    );

    $("#linkUserLoginForm").data("dataTable", _dataTable);

    $("#CreateLinkUser").click(function () {
        abp.message.confirm(l("NewLinkAccountWarning"), l("AreYouSure"), function (result) {
            if(result) {
                var loginUrl = $("#LinkUserLoginUrl").val();
                var returnUrl = $("#linkUserLoginForm input[name=ReturnUrl]").val();
                _identityLinkUser.generateLinkToken().then(function (token) {
                    var url =
                        loginUrl +
                        "Account/Login?handler=CreateLinkUser&" +
                        "LinkUserId=" +
                        abp.currentUser.id +
                        "&LinkToken=" +
                        encodeURIComponent(token) +
                        "&ReturnUrl=" + returnUrl;
                    if (abp.currentTenant.id) {
                        url += "&LinkTenantId=" + abp.currentTenant.id;
                    }
                    location.href = url;
                });
            }
        });
    });

})();
