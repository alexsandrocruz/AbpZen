$(function () {
    var l = abp.localization.getResource("CmsKit");

    var urlShortingService = volo.cmsKit.admin.urlShorting.urlShortingAdmin;

    var createModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/UrlShorting/CreateModal",
        modalClass: "CreateUrlShortingModal"
    });
    var editModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/UrlShorting/EditModal",
        modalClass: "EditUrlShortingModal"
    });

    var getFilter = function () {
        return {
            shortenedUrlFilter: $("#Filter").val()
        };
    };

    let dataTable = $("#UrlShortingTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        searching: false,
        processing: true,
        scrollX: true,
        serverSide: true,
        paging: true,
        ajax: abp.libs.datatables.createAjax(urlShortingService.getList, getFilter),
        columnDefs: [
            {
                title: l("Actions"),
                targets: 0,
                width: "20%",
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.UrlShorting.Update'),
                            action: function (data) {
                                editModal.open({
                                    id: data.record.id
                                });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.UrlShorting.Delete'),
                            confirmMessage: function (data) {
                                return l("ForwardedUrlDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                urlShortingService.delete(data.record.id)
                                .then(function () {
                                    dataTable.ajax.reloadEx();
                                    abp.notify.success(l('DeletedSuccessfully'));
                                });
                            }
                        }
                    ]
                }
            },
            {
                width: "30%",
                title: l("Source"),
                data: "source"
            },
            {
                width: "50%",
                title: l("Target"),
                data: "target"
            }
        ]
    }));

    editModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });
    createModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });

    $("#RefreshFilterButton").on("click", "", function () {
        dataTable.ajax.reloadEx();
    });

    $("#NewShortenedUrlButton").on("click", "", function () {
        createModal.open({});
    });

    $("#Filter").keypress(function (event) {
        let keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == "13") {
            dataTable.ajax.reloadEx();
        }
    })
});
