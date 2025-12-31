(function ($) {

    var l = abp.localization.getResource('AbpOpenIddict');

    var _createModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'OpenIddict/Applications/CreateModal',
        scriptUrl: abp.appPath + "Pages/OpenIddict/Applications/createModal.js",
        modalClass: 'applicationCreate'
    });

    _createModal.onResult(function(){
        abp.notify.success(l('CreatedSuccessfully'));
    });
    
    var _editModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'OpenIddict/Applications/EditModal',
        modalClass: 'applicationUpdate'
    });

    _editModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _tokenLifetimeModal = new abp.ModalManager({
          viewUrl: abp.appPath + 'OpenIddict/Applications/TokenLifetimeModal'
    });

    _tokenLifetimeModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });

    var _permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');

    _permissionsModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _dataTable = null;

    abp.ui.extensions.entityActions.get("openIddict.Application").addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('OpenIddictPro.Application.Update'),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('TokenLifetime'),
                        visible: abp.auth.isGranted('OpenIddictPro.Application.Update'),
                        action: function (data) {
                            _tokenLifetimeModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Permissions'),
                        visible: abp.auth.isGranted('OpenIddictPro.Application.ManagePermissions'),
                        action: function (data) {
                            _permissionsModal.open({
                                providerName: 'C',
                                providerKey: data.record.clientId,
                                providerKeyDisplayName: data.record.displayName
                            });
                        }
                    },
                    {
                        text: l('ChangeHistory'),
                        visible: abp.auditLogging !== undefined && abp.auth.isGranted('AuditLogging.ViewChangeHistory:Volo.Abp.OpenIddict.Pro.Applications.Application'),
                        action: function (data) {
                            abp.auditLogging.openEntityHistoryModal(
                                "Volo.Abp.OpenIddict.Applications.OpenIddictApplication",
                                data.record.id
                            );
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('OpenIddictPro.Application.Delete'),
                        confirmMessage: function (data) {
                            return l('ApplicationDeletionWarningMessage', data.record.clientId);
                        },
                        action: function (data) {
                            volo.abp.openIddict.application
                                .delete(data.record.id)
                                .then(function () {
                                    _dataTable.ajax.reloadEx();
                                    abp.notify.success(l('DeletedSuccessfully'));
                                });
                        }
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get("openIddict.Application").addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get("openIddict.Application").actions.toArray()
                        }
                    },
                    {
                        title: l("ApplicationType"),
                        data: "applicationType"
                    },
                    {
                        title: l("ClientId"),
                        data: "clientId"
                    },
                    {
                        title: l("DisplayName"),
                        data: "displayName"
                    },
                    {
                        title: l("ClientType"),
                        data: "clientType"
                    }
                ]
            );
        },
        0
    );

    $(function () {

        var _$wrapper = $('#ApplicationsWrapper');

        var getFilter = function () {
            return {
                filter: _$wrapper.find('input.page-search-filter-text').val()
            };
        };

        _dataTable = $('#ApplicationsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            scrollX: true,
            searching: false,
            scrollCollapse: true,
            order: [[1, "desc"]],
            ajax: abp.libs.datatables.createAjax(volo.abp.openIddict.application.getList, getFilter),
            columnDefs: abp.ui.extensions.tableColumns.get("openIddict.Application").columns.toArray()
        }));

        $("#CreateNewButtonId").click(function () {
            _createModal.open();
        });

        _createModal.onClose(function () {
            _dataTable.ajax.reloadEx();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _$wrapper.find('form.page-search-form').submit(function (e) {
            e.preventDefault();
            _dataTable.ajax.reloadEx();
        });
    });
})(jQuery);
