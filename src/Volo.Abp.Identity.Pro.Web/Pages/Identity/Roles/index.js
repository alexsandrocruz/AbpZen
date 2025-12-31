(function ($) {

    var l = abp.localization.getResource('AbpIdentity');
    var _identityRoleAppService = volo.abp.identity.identityRole;
    var _permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');

    _permissionsModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _claimTypeEditModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Identity/Roles/ClaimTypeEditModal',
        modalClass: 'claimTypeEdit'
    });

    _claimTypeEditModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _editModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/EditModal');

    _editModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _createModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/CreateModal');

    _createModal.onResult(function(){
        abp.notify.success(l('CreatedSuccessfully'));
    });
    
    var _deleteRoleModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/DeleteRoleModal');

    _deleteRoleModal.onResult(function(){
        abp.notify.success(l('DeletedSuccessfully'));
    });
    
    _deleteRoleModal.onOpen(function () {
        var $form = _deleteRoleModal.getForm();
        $form.find('#assign').click(function () {
            $form.find('#Role_AssignToRoleId').show();
            $form.find('[type=submit]').attr("disabled","disabled")
        })
        $form.find('#unassign').click(function () {
            $form.find('#Role_AssignToRoleId').hide();
            $form.find('#Role_AssignToRoleId').val("");
            $form.find('[type=submit]').removeAttr("disabled");
        })

        $("#Role_AssignToRoleId").on("change", function () {
            var val = $(this).val();
            if(val === ''){
                $form.find('[type=submit]').attr("disabled","disabled")
            }else{
                $form.find('[type=submit]').removeAttr("disabled");
            }
        })
    })
    var _moveAllUsersModal = new abp.ModalManager(abp.appPath + 'Identity/Roles/MoveAllUsersModal');

    _moveAllUsersModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });

    var _dataTable = null;

    abp.ui.extensions.entityActions.get("identity.role").addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('AbpIdentity.Roles.Update'),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Claims'),
                        visible: abp.auth.isGranted('AbpIdentity.Roles.Update'),
                        action: function (data) {
                            _claimTypeEditModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Permissions'),
                        visible: abp.auth.isGranted('AbpIdentity.Roles.ManagePermissions'),
                        action: function (data) {
                            _permissionsModal.open({
                                providerName: 'R',
                                providerKey: data.record.name,
                                providerKeyDisplayName: data.record.name
                            });
                        }
                    },
                    {
                        text: l('ChangeHistory'),
                        visible: abp.auditLogging !== undefined && abp.auth.isGranted('AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityRole'),
                        action: function (data) {
                            abp.auditLogging.openEntityHistoryModal(
                                "Volo.Abp.Identity.IdentityRole",
                                data.record.id
                            );
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('AbpIdentity.Roles.Delete'),
                        action: function (data) {
                           _deleteRoleModal.open({
                               id: data.record.id
                           });
                        }
                    },
                    {
                        text: l('MoveAllUsers'),
                        visible: abp.auth.isGranted('AbpIdentity.Roles.Update'),
                        action: function (data) {
                            if(data.record.userCount === 0){
                                abp.message.warn(l('ThereIsNoUsersCurrentlyInThisRole'));
                                return;
                            }
                            _moveAllUsersModal.open({
                                id: data.record.id
                            });
                        }
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get("identity.role").addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l('Actions'),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get("identity.role").actions.toArray()
                        }
                    },
                    {
                        title: l('RoleName'),
                        data: "name",
                        render: function (data, type, row) {
                            var name = '<span>' + $.fn.dataTable.render.text().display(data) + '</span>'; //prevent against possible XSS

                            if (row.isDefault) {
                                name += '<span class="badge rounded-pill ms-1 bg-success">' + l('DisplayName:IsDefault') + '</span>';
                            }

                            if (row.isPublic) {
                                name += '<span class="badge rounded-pill ms-1 bg-info">' + l('DisplayName:IsPublic') + '</span>';
                            }

                            return name;
                        }
                    },
                    {
                        title: l('UserCount'),
                        data: "userCount",
                        orderable: false
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {

        var getFilter = function () {
            return {
                filter: $('#IdentityRolesWrapper input.page-search-filter-text').val()
            };
        };

        _dataTable = $('#IdentityRolesWrapper table')
            .DataTable(abp.libs.datatables.normalizeConfiguration({
                order: [[1, "asc"]],
                searching: false,
                processing: true,
                scrollX: true,
                serverSide: true,
                paging: true,
                ajax: abp.libs.datatables.createAjax(_identityRoleAppService.getList, getFilter),
                columnDefs: abp.ui.extensions.tableColumns.get("identity.role").columns.toArray()
            }));

        _createModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _deleteRoleModal.onResult(function () {
           _dataTable.ajax.reloadEx();
       });

        _moveAllUsersModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        })

        _moveAllUsersModal.onOpen(function () {
            var $form = _moveAllUsersModal.getForm();
            $form.find('#Role_CurrentRoleId').change(function () {
                $form.find('#MoveAllUsersWithRoleTo').html(l('MoveAllUsersWithRoleTo', this.selectedOptions[0].text));
            })
        })

        $('#AbpContentToolbar button[name=CreateRole]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });

        $('#IdentityRolesWrapper form.page-search-form').submit(function (e) {
            e.preventDefault();
            _dataTable.ajax.reloadEx();
        });
    });

})(jQuery);
