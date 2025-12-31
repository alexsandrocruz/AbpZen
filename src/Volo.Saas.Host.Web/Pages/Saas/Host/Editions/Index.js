(function () {

    var l = abp.localization.getResource('Saas');
    var _editionAppService = volo.saas.host.edition;

    var _editModal = new abp.ModalManager(abp.appPath + 'Saas/Host/Editions/EditModal');

    _editModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _createModal = new abp.ModalManager(abp.appPath + 'Saas/Host/Editions/CreateModal');

    _createModal.onResult(function(){
        abp.notify.success(l('CreatedSuccessfully'));
    });
    
    var _featuresModal = new abp.ModalManager(abp.appPath + 'FeatureManagement/FeatureManagementModal');

    _featuresModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _deleteEditionModal = new abp.ModalManager(abp.appPath + 'Saas/Host/Editions/DeleteEditionModal');

    _deleteEditionModal.onResult(function(){
        abp.notify.success(l('DeletedSuccessfully'));
    });
    
    _deleteEditionModal.onOpen(function () {
        var $form = _deleteEditionModal.getForm();
        $form.find('#assign').click(function () {
            $form.find('#Edition_AssignToEditionId').show();
            $form.find('[type=submit]').attr("disabled","disabled")
        })
        $form.find('#unassign').click(function () {
            $form.find('#Edition_AssignToEditionId').hide();
            $form.find('#Edition_AssignToEditionId').val("");
            $form.find('[type=submit]').removeAttr("disabled");
        })
        
        $("#Edition_AssignToEditionId").on("change", function () {
            var val = $(this).val();
            if(val === ''){
                $form.find('[type=submit]').attr("disabled","disabled")
            }else{
                $form.find('[type=submit]').removeAttr("disabled");
            }
        })
    })

    var _moveAllTenantsModal = new abp.ModalManager(abp.appPath + 'Saas/Host/Editions/MoveAllTenantsModal');

    _moveAllTenantsModal.onResult(function(){
        abp.notify.success(l('SavedSuccessfully'));
    });
    
    var _dataTable = null;

    abp.ui.extensions.entityActions.get("saas.edition").addContributor(
        function(actionList) {
            actionList.addManyTail(
                [
                    {
                        text: l('Edit'),
                        visible: abp.auth.isGranted('Saas.Editions.Update'),
                        action: function (data) {
                            _editModal.open({
                                id: data.record.id
                            });
                        }
                    },
                    {
                        text: l('Features'),
                        visible: abp.auth.isGranted('Saas.Editions.ManageFeatures'),
                        action: function (data) {
                            _featuresModal.open({
                                providerName: 'E',
                                providerKey: data.record.id,
                                providerKeyDisplayName: data.record.displayName
                            })
                        }
                    },
                    {
                        text: l('ChangeHistory'),
                        visible: abp.auditLogging && abp.auth.isGranted('AuditLogging.ViewChangeHistory:Volo.Saas.Edition'),
                        action: function (data) {
                            abp.auditLogging.openEntityHistoryModal(
                                "Volo.Saas.Edition",
                                data.record.id
                            );
                        }
                    },
                    {
                        text: l('Delete'),
                        visible: abp.auth.isGranted('Saas.Editions.Delete'),
                        action: function (data) {
                           _deleteEditionModal.open({
                               id: data.record.id
                           });
                        }
                    },
                    {
                        text: l('MoveAllTenants'),
                        visible: abp.auth.isGranted('Saas.Editions.Update'),
                        action: function (data) {
                            if (data.record.tenantCount == 0) {
                                abp.message.warn(l('ThereIsNoTenantsCurrentlyInThisEdition'));
                                return;
                            }
                            _moveAllTenantsModal.open({
                                id: data.record.id
                            });
                        }
                    }
                ]
            );
        }
    );

    abp.ui.extensions.tableColumns.get("saas.edition").addContributor(
        function (columnList) {
            columnList.addManyTail(
                [
                    {
                        title: l("Actions"),
                        rowAction: {
                            items: abp.ui.extensions.entityActions.get("saas.edition").actions.toArray()
                        }
                    },
                    {
                        title: l("EditionName"),
                        data: "displayName"
                    },
                    {
                        title: l("PlanName"),
                        data: "planName",
                        orderable: false
                    },
                    {
                        title: l("TenantCount"),
                        data: "tenantCount",
                        orderable: false
                    }
                ]
            );
        },
        0 //adds as the first contributor
    );

    $(function () {

        var _$wrapper = $('#EditionsWrapper');

        var getFilter = function () {
            return {
                filter: _$wrapper.find("input.page-search-filter-text").val()
            };
        };

        _dataTable = _$wrapper.find('table').DataTable(abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: true,
            paging: true,
            order: [[1, "asc"]],
            searching: false,
            scrollX: true,
            ajax: abp.libs.datatables.createAjax(_editionAppService.getList, getFilter),
            columnDefs: abp.ui.extensions.tableColumns.get("saas.edition").columns.toArray()
        }));

        _createModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _editModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        });

        _deleteEditionModal.onResult(function () {
           _dataTable.ajax.reloadEx();
       });

        _moveAllTenantsModal.onResult(function () {
            _dataTable.ajax.reloadEx();
        })

        $('#AbpContentToolbar button[name=CreateEdition]').click(function (e) {
            e.preventDefault();
            _createModal.open();
        });

        _$wrapper.find("form.page-search-form").submit(function (e) {
            e.preventDefault();
            _dataTable.ajax.reloadEx();
        });
    });

})();
