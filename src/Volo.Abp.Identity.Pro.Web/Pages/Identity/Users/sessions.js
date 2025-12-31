var abp = abp || {};

$(function () {

  var l = abp.localization.getResource("AbpIdentity");
  var _identitySession = volo.abp.identity.identitySession;
  var _dataTable = null;
  var _detailModal = new abp.ModalManager(abp.appPath + 'Identity/Users/SessionDetail');
  _detailModal.onClose(function () {
    abp.ui.unblock("#UserSessionModal");
  });

  abp.ui.extensions.entityActions.get("identity.sessions").addContributor(
    function (actionList) {
      return actionList.addManyTail(
        [
          {
            text: l("Session:Detail"),
            action: function (data) {
              abp.ui.block("#UserSessionModal");
              _detailModal.open({
                id: data.record.id
              });
            }
          },
          {
            text: l("Session:Revoke"),
            confirmMessage: function (data) {
              return l('SessionRevokeConfirmationMessage');
            },
            action: function (data) {
              _identitySession.revoke(data.record.id).then(function () {
                if (data.record.isCurrent) {
                  location.assign("/");
                }
                else {
                  _dataTable.ajax.reload();
                }
              });
            }
          }
        ]
      );
    }
  );

  abp.ui.extensions.tableColumns.get("identity.sessions").addContributor(
    function (columnList) {
      columnList.addManyTail([
        {
          title: l("Actions"),
          rowAction:
          {
            items: abp.ui.extensions.entityActions.get("identity.sessions").actions.toArray()
          }
        },
        {
          title: l("Session:Device"),
          data: "device",
          autoWidth: true,
          render: function (data, type, row) {
            var text = data;
            if (row.isCurrent) {
              text += ' <i class="fas fa-dot-circle" data-bs-toggle="tooltip" data-bs-placement="right" data-bs-title="' + l("Session:Current") + '"></i>';
            }
            return text;
          },
        },
        {
          title: l("Session:DeviceInfo"),
          data: "deviceInfo",
          autoWidth: true
        },
        {
          title: l("Session:SignedIn"),
          data: "signedIn",
          dataFormat: "datetime",
          autoWidth: true
        },
        {
          title: l("Session:LastAccessed"),
          data: "lastAccessed",
          dataFormat: "datetime",
          autoWidth: true
        }
      ]);
    },
    0 //adds as the first contributor
  );

  abp.modals.sessions = function () {
    var initModal = function (publicApi, args) {
      var getFilter = function () {
        return {
          userId: $('#IdentityUserSessionWrapper input[name=UserId]').val()
        };
      };

      _dataTable = $("#IdentityUserSessionsTable").DataTable(
        abp.libs.datatables.normalizeConfiguration({
          processing: true,
          serverSide: true,
          searching: false,
          scrollX: true,
          paging: true,
          order: [[4, "desc"]],
          ajax: abp.libs.datatables.createAjax(
            _identitySession.getList,
            getFilter
          ),
          columnDefs: abp.ui.extensions.tableColumns
            .get("identity.sessions")
            .columns.toArray(),
        })
      );
      publicApi.onOpen(function () {
        _dataTable.columns.adjust();
      });
    };
    return {
      initModal: initModal
    };
  };
});
