$(function () {
  var _dataTable = null;
  var l = abp.localization.getResource("AbpAccount");
  var _accountExternalLogin = volo.abp.account.accountExternalLogin;
  abp.ui.extensions.entityActions.get("account.externalLogins").addContributor(
    function (actionList) {
      return actionList.addManyTail(
        [
          {
            text: l("Delete"),
            confirmMessage: function (data) {
              return l(
                'ExternalLoginDeleteConfirmationMessage',
                data.record.loginProvider)
            },
            action: function (data) {
              _accountExternalLogin.delete(data.record.loginProvider, data.record.providerKey).then(function () {
                location.reload();
              });
            }
          }
        ]
      );
    }
  );

  abp.ui.extensions.tableColumns.get("account.externalLogins").addContributor(
    function (columnList) {
      columnList.addManyTail([
        {
          title: l("Actions"),
          rowAction:
          {
            items: abp.ui.extensions.entityActions.get("account.externalLogins").actions.toArray()
          }
        },
        {
          title: l("ExternalLogin:LoginProvider"),
          data: "loginProvider",
          autoWidth: true,
          orderable: false
        },
        {
          title: l("ExternalLogin:ProviderDisplayName"),
          data: "providerDisplayName",
          autoWidth: true,
          orderable: false
        }
      ]);
    },
    0
  );

  _dataTable = $("#ExternalLoginsTable").DataTable(
    abp.libs.datatables.normalizeConfiguration({
      processing: true,
      serverSide: true,
      searching: false,
      paging: false,
      info: false,
      order: [],
      ajax: function () {
        return function (requestData, callback, settings) {
          if (callback) {
            _accountExternalLogin.getList().then(function (result) {
              callback({
                recordsTotal: result.length,
                recordsFiltered: result.length,
                data: result
              });
            });
          }
        }
      }(),
      columnDefs: abp.ui.extensions.tableColumns
        .get("account.externalLogins")
        .columns.toArray(),
    })
  );

  $("#newExternalLoginForm").submit(function (e) {
    abp.ui.block({
      elm: '#externalLoginsModal',
      busy: true
    });
    return true;
  });

});
