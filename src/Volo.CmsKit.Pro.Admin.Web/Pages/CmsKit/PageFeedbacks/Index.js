$(function () {
    var l = abp.localization.getResource("CmsKit");

    var pageFeedbackService = volo.cmsKit.admin.pageFeedbacks.pageFeedbackAdmin;

    pageFeedbackService.getEntityTypes().then(function (data) {
        var entityTypeSelect = $("#EntityType");
        entityTypeSelect.empty();
        entityTypeSelect.append($("<option></option>").val("").html("-"));
        $.each(data, function (index, entityType) {
            entityTypeSelect.append($("<option></option>").val(entityType).html(entityType));
        });
    });

    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/PageFeedbacks/EditModal",
        modalClass: 'editPageFeedback'
    });
    
    var viewModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/PageFeedbacks/ViewModal",
        modalClass: 'viewPageFeedback'
    });

    var settingsModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/PageFeedbacks/SettingsModal",
        modalClass: 'settingsPageFeedback'
    });

    var getFilter = function () {
        return {
            entityType: $("#EntityType").val() || "",
            entityId: $("#EntityId").val() || "",
            isHandled: $("#IsHandled").val(),
            isUseful: $("#IsUseful").val(),
            url: $("#UrlFilter").val(),
            hasUserNote: $("#HasUserNote").val(),
            hasAdminNote: $("#HasAdminNote").val()
        };
    };

    let dataTable = $("#PageFeedbacksTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        searching: false,
        processing: true,
        scrollX: true,
        serverSide: true,
        paging: true,
        ajax: abp.libs.datatables.createAjax(pageFeedbackService.getList, getFilter),
        columnDefs: [
            {
                title: l("Actions"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('View'),
                            action: function (data) {
                                viewModal.open({
                                    id: data.record.id
                                });
                            }
                        },
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.PageFeedback.Update'),
                            action: function (data) {
                                editModal.open({
                                    id: data.record.id
                                });
                            }
                        },

                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.PageFeedback.Delete'),
                            confirmMessage: function (data) {
                                return l("PageFeedbackDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                pageFeedbackService.delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reloadEx();
                                        abp.notify.success(l('DeletedSuccessfully'));
                                    });
                            }
                        },
                    ]
                }
            },
            {
                title: l("EntityType"),
                data: "entityType"
            },
            {
                title: l("EntityId"),
                data: "entityId"
            },
            {
                title: l('UserMessage'),
                data: "userNote",
                render: function (data, type, row) {
                    if (row.userNote) {
                        return '<button class="border-0 bg-transparent d-flex justify-content-center align-items-center message" data-messageField="userNote"><i class="fas fa-comment"></i></button>';
                    }
                    return null;
                }
            },
            {
                title: l('AdminMessage'),
                data: "adminNote",
                render: function (data, type, row) {
                    if (row.adminNote) {
                        return '<button class="border-0 bg-transparent d-flex justify-content-center align-items-center message" data-messageField="adminNote"><i class="fas fa-comment"></i></button>';
                    }
                    return null;
                }
            },
            {
                title: l("Url"),
                data: "url",
                render: function (data, type, row, meta) {
                    return `<a href="${data}" target="_blank">${data.substring(0, 128)}</a>`;
                }
            },
            {
                title: l("Useful"),
                data: "isUseful",
                render: function (data, type, row, meta) {
                    if (data) {
                        return `<i class="fa fa-thumbs-up text-success"></i>`;
                    }
                    return `<i class="fa fa-thumbs-down text-danger"></i>`;
                }
            },
            {
                title: l("Handled"),
                data: "isHandled",
                render: function (data, type, row, meta) {
                    if (data) {
                        return `<i class="fa fa-check text-success"></i>`;
                    }
                    return ``;
                }
            },
            {
                title: l("CreationTime"),
                data: "creationTime",
                dataFormat: "datetime",
            }
        ]
    }));
    
    $(document).on('click', 'button.message', function () {
        var messageField = $(this).data('messagefield');
        if (!messageField) {
            return;
        }
        var row = dataTable.row($(this).closest('tr')).data();
        var modal = $('#message-modal');
        modal.find('.modal-title').text(messageField === 'userNote' ? l('UserMessage') : l('AdminMessage'));
        modal.find('#message-body').val(row[messageField]);
        modal.modal('show');
    });

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });

    $("#RefreshFilterButton").on("click", "", function () {
        dataTable.ajax.reloadEx();
    })

    $('#FilterSection input')
        .on('change', function () {
            dataTable.ajax.reloadEx();
        })
        .on('keypress', function (e) {
            if (e.which === 13) {
                dataTable.ajax.reloadEx();
            }
        });

    $('#FilterSection select').on('change', function () {
        dataTable.ajax.reloadEx();
    });

    $("#SettingsButton").on("click", "", function () {
        settingsModal.open();
    });
});
