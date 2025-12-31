(function ($) {

    let l = abp.localization.getResource('AbpIdentity');

    let _identityUserAppService = volo.abp.identity.identityUser;
    
    let _claimTypeEditModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/ClaimTypeEditModal', modalClass: 'claimTypeEdit' });

    _claimTypeEditModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _editModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/EditModal', modalClass: 'editUser' });

    _editModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _createModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/CreateModal', modalClass: 'createUser' });

    _createModal.onResult(function(){
        abp.notify.success(l('CreatedSuccessfully'));
    });
    
    let _viewDetailsModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/ViewDetailsModal', modalClass: 'viewDetailsUser' });

    _viewDetailsModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _permissionsModal = new abp.ModalManager(abp.appPath + 'AbpPermissionManagement/PermissionManagementModal');

    _permissionsModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _changeUserPasswordModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/SetPassword', modalClass: 'changeUserPassword' });

    _changeUserPasswordModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _lockoutModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/Lock', modalClass: 'lock' });

    _lockoutModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    let _twoFactorModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/TwoFactor', modalClass: 'twoFactor' });

    _twoFactorModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });

    let _sessionsModal = new abp.ModalManager({ viewUrl: abp.appPath + 'Identity/Users/SessionsModal', modalClass: 'sessions' });

    let _dataTable = null;

    abp.ui.extensions.entityActions.get("identity.user").addContributor(
        function (actionList) {
            return actionList.addManyTail(
                [
                    {
                        text: l('ViewDetails'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.ViewDetails'),
                        action: function (data) {
                            _viewDetailsModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.Update'),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Claims'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.Update'),
                        action: function (data) {
                            _claimTypeEditModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Lock'),
                        visible: function (data) {
                            return abp.currentUser.id != data.id &&
                                data.lockoutEnabled &&
                                abp.auth.isGranted('AbpIdentity.Users.Update');
                        },
                        action: function (data) {
                            _lockoutModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Unlock'),
                        visible: function (data) {
                            return abp.currentUser.id != data.id &&
                                data.isLockedOut &&
                                abp.auth.isGranted('AbpIdentity.Users.Update'); //TODO: New permission for lockout?
                        },
                        action: function (data) {
                            _identityUserAppService
                                .unlock(data.record.id)
                                .then(function () {
                                    abp.notify.success(l("UserUnlocked"));
                                    _dataTable.ajax.reloadEx();
                                });
                        }
                    },
                    {
                        text: l('Permissions'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.ManagePermissions'),
                        action: function (data) {
                            _permissionsModal.open({
                                providerName: 'U',
                                providerKey: data.record.id,
                                providerKeyDisplayName: data.record.userName
                            });
                        }
                    },
                    {
                        text: l('SetPassword'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.Update'),
                        action: function (data) {
                            _changeUserPasswordModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('TwoFactor'),
                        visible: function (data) {
                            return abp.auth.isGranted('AbpIdentity.Users.Update') && data.supportTwoFactor;
                        },
                        action: function (data) {
                            _twoFactorModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Sessions'),
                        visible: abp.auth.isGranted('AbpIdentity.Users.Update') ,
                        action: function (data) {
                            _sessionsModal.open({
                                userId: data.record.id
                            });
                        }
                    },
                    {
                        text: l('ChangeHistory'),
                        visible: abp.auditLogging !== undefined && abp.auth.isGranted('AuditLogging.ViewChangeHistory:Volo.Abp.Identity.IdentityUser'),
                        action: function (data) {
                            abp.auditLogging.openEntityHistoryModal(
                                "Volo.Abp.Identity.IdentityUser",
                                data.record.id
                            );
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: function (data) {
                            return abp.auth.isGranted('AbpIdentity.Users.Delete') && abp.currentUser.id !== data.id;
                        },
                        confirmMessage: function (data) {
                            return l('UserDeletionConfirmationMessage', data.record.userName);
                        },
                        action: function (data) {
                            _identityUserAppService
                                .delete(data.record.id)
                                .then(function () {
                                    abp.notify.success(l("DeletedSuccessfully"));
                                    _dataTable.ajax.reloadEx();
                                });
                        }
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get("identity.user").addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l('Actions'),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get("identity.user").actions.toArray()
                        }
                    },
                    {
                        title: l('UserName'),
                        autoWidth: false,
                        data: 'userName',
                        render: function (data, type, row) {

                            row.userName = $.fn.dataTable.render.text().display(row.userName);
                            let roleHtml = row.userName;
                            let lockedOutHtml = '';
                            let notActiveHtml = '';
                            if (row.isLockedOut) {
                                lockedOutHtml = '<i data-toggle="tooltip" data-placement="top" title="' +
                                    l('ThisUserIsLockedOutMessage') +
                                    '" class="fa fa-lock"></i> ';
                            }

                            if (!row.isActive) {
                                notActiveHtml = '<i data-toggle="tooltip" data-placement="top" title="' +
                                    l('ThisUserIsNotActiveMessage') +
                                    '" class="fa fa-ban text-danger"></i> ';
                            }

                            if (row.isLockedOut || !row.isActive) {
                                roleHtml = lockedOutHtml + notActiveHtml + '<span class="opc-65">' + row.userName + '</span>';
                            }

                            return roleHtml;
                        }
                    },
                    {
                        title: l('EmailAddress'),
                        data: "email",
                        render: function (data, type, row) {
                            if (!row.emailConfirmed) {
                                return data;
                            }

                            return data + ' <i class="text-success ms-1 fa fa-check" data-toggle="tooltip" data-placement="top" title="' + l('Verified') + '"></i>';
                        }
                    },
                    {
                        title: l("Roles"),
                        data: "roleNames",
                        orderable: false,
                        render: function (data, type, row) {
                            if (!data || !Array.isArray(data)) {
                                return "";
                            }

                            return data.join(", ");
                        }
                    },
                    {
                        title: l('PhoneNumber'),
                        data: "phoneNumber",
                        render: function (data, type, row) {
                            if (!row.phoneNumberConfirmed) {
                                return data;
                            }

                            return data + ' <i class="text-success ms-1 fa fa-check" data-toggle="tooltip" data-placement="top" title="' + l('Verified') + '"></i>';
                        }
                    },
                    {
                        title: l('Name'),
                        data: "name",
                    },
                    {
                        title: l('Surname'),
                        data: "surname",
                    },
                    {
                        title: l('DisplayName:IsActive'),
                        data: "isActive",
                        render: function (data) {
                            if (data) {
                                return '<i class="fa fa-check"></i>';
                            } else {
                                return '<i class="fa fa-ban"></i>';
                            }
                        }
                    },
                    {
                        title: l('DisplayName:LockoutEnabled'),
                        data: "lockoutEnabled",
                        render: function (data) {
                            if (data) {
                                return '<i class="fa fa-check"></i>';
                            } else {
                                return '<i class="fa fa-ban"></i>';
                            }
                        }
                    },
                    {
                        title: l('DisplayName:EmailConfirmed'),
                        data: "emailConfirmed",
                        render: function (data) {
                            if (data) {
                                return '<i class="fa fa-check"></i>';
                            } else {
                                return '<i class="fa fa-ban"></i>';
                            }
                        }
                    },
                    {
                        title: l('DisplayName:TwoFactorEnabled'),
                        data: "twoFactorEnabled",
                        render: function (data) {
                            if (data) {
                                return '<i class="fa fa-check"></i>';
                            } else {
                                return '<i class="fa fa-ban"></i>';
                            }
                        }
                    },
                    {
                        title: l('DisplayName:AccessFailedCount'),
                        data: "accessFailedCount",
                    },
                    {
                        title: l('CreationTime'),
                        data: "creationTime",
                        dataFormat: "datetime"
                    },
                    {
                        title: l('LastModificationTime'),
                        data: "lastModificationTime",
                        dataFormat: "datetime"
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    moment()._locale.preparse = (string) => string;
    moment()._locale.postformat = (string) => string;

    let getFormattedDate = function ($daterangesingle) {
        return moment($daterangesingle.val(), 'l', true).isValid() ? moment($daterangesingle.val(), 'l').format('YYYY-MM-DD') : null;
    };

    $('.singledatepicker').daterangepicker({
        "singleDatePicker": true,
        "showDropdowns": true,
        "autoUpdateInput": false,
        "autoApply": true,
        "opens": "center",
        "drops": "auto"
    });

    $('.singledatepicker').attr('autocomplete', 'off');

    $('.singledatepicker').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('l'));
        $(this).trigger('change');
    });

    $('.singledatepicker').on('change', function () {
        _dataTable.ajax.reloadEx();
    });

    $(function () {

        let getFilter = function () {
            let roleId = $("#IdentityUsersWrapper select#IdentityRole").val();
            let organizationUnitId = $("#IdentityUsersWrapper select#OrganizationUnit").val();

            return {
                filter: $('#IdentityUsersWrapper input.page-search-filter-text').val(),
                roleId: !roleId ? null : roleId,
                organizationUnitId: !organizationUnitId ? null : organizationUnitId,
                userName: $("#IdentityUsersWrapper input#UserName").val(),
                phoneNumber: $("#IdentityUsersWrapper input#PhoneNumber").val(),
                emailAddress: $("#IdentityUsersWrapper input#EmailAddress").val(),
                tenantName: $("#IdentityUsersWrapper input#TenantName").val(),
                name: $("#IdentityUsersWrapper input#Name").val(),
                surname: $("#IdentityUsersWrapper input#Surname").val(),
                maxCreationTime: getFormattedDate($("#MaxCreationTime")),
                minCreationTime: getFormattedDate($("#MinCreationTime")),
                maxModifitionTime: getFormattedDate($("#MaxModificationTime")),
                minModifitionTime: getFormattedDate($("#MinModificationTime")),
                isLockedOut: $("#IdentityUsersWrapper select#IsLockedOut").val(),
                notActive: $("#IdentityUsersWrapper select#NotActive").val(),
                emailConfirmed: $("#IdentityUsersWrapper select#EmailConfirmed").val(),
                isExternal: $("#IdentityUsersWrapper select#IsExternal").val()
            };
        };

        _dataTable = $('#IdentityUsersWrapper table').DataTable(abp.libs.datatables.normalizeConfiguration({
            order: [[2, "asc"]],
            processing: true,
            serverSide: true,
            searching: false,
            scrollX: true,
            paging: true,
            ajax: abp.libs.datatables.createAjax(_identityUserAppService.getList, getFilter),
            columnDefs: abp.ui.extensions.tableColumns.get("identity.user").columns.toArray()
        }));

        $('#IdentityUsersWrapper table').data('dataTable', _dataTable);

        _createModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _lockoutModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        $('#AbpContentToolbar button[name=CreateUser]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });

        $('#IdentityUsersWrapper form.page-search-form').submit(function (e) {
            e.preventDefault();
            _dataTable.ajax.reloadEx();
        });

        $("#IdentityUsersWrapper select#IdentityRole," +
            "#IdentityUsersWrapper select#OrganizationUnit," +
            "#IdentityUsersWrapper select#Tenant," +
            "#IdentityUsersWrapper select#IsLockedOut," +
            "#IdentityUsersWrapper select#NotActive," +
            "#IdentityUsersWrapper select#EmailConfirmed," +
            "#IdentityUsersWrapper select#IsExternal"
        ).change(function (e) {
            _dataTable.ajax.reloadEx();
        });

        $("#AdvancedFilterSectionToggler").click(function (e) {
            $("#AdvancedFilterSection").toggle();
            var iconCss = $("#AdvancedFilterSection").is(":visible") ? "fa ms-1 fa-angle-up" : "fa ms-1 fa-angle-down";
            $(this).find("i").attr("class", iconCss);
        });

        $("#IdentityUsersWrapper input#UserName," +
            "#IdentityUsersWrapper input#EmailAddress," +
            "#IdentityUsersWrapper input#PhoneNumber," +
            "#IdentityUsersWrapper input#Name," +
            "#IdentityUsersWrapper input#MaxCreationTime," +
            "#IdentityUsersWrapper input#MinCreationTime," +
            "#IdentityUsersWrapper input#MaxModificationTime," +
            "#IdentityUsersWrapper input#MinModificationTime," +
            "#IdentityUsersWrapper input#Surname"
        ).keypress(function (e) {
            if (e.which === 13) {
                _dataTable.ajax.reloadEx();
            }
        });
    });

})(jQuery);
