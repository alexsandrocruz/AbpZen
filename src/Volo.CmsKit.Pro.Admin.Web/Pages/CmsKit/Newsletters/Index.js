$(function () {
    var l = abp.localization.getResource("CmsKit");

    var newsletterService = volo.cmsKit.admin.newsletters.newsletterRecordAdmin;

    var detailModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Newsletters/Detail",
        modalClass: "newsletterDetailModel"
    });
    
    var editPreferencesModal = new abp.ModalManager({
        viewUrl: abp.appPath + "CmsKit/Newsletters/EditPreferences",
        modalClass: "EditPreferencesModel",
        scriptUrl: abp.appPath + "Pages/CmsKit/Newsletters/EditPreferences.js"
    });

    var getFilter = function () {
        return {
            preference: $("#AdvancedFilterSection #Preference").val(),
            source: $("#AdvancedFilterSection #Source").val(),
            emailAddress: $("#AdvancedFilterSection #EmailAddress").val()
        };
    };

    let dataTable = $("#NewslettersTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        scrollX: true,
        ordering: false,
        ajax: abp.libs.datatables.createAjax(newsletterService.getList, getFilter),
        columnDefs: [
            {
                title: l("Details"),
                targets: 0,
                rowAction: {
                    items: [
                        {
                            text: l('Detail'),
                            action: function (data) {
                                detailModal.open({
                                    id: data.record.id
                                });
                            }
                        },
                        {
                            text: l('EditPreferences'),
                            action: function (data) {
                                editPreferencesModal.open({
                                    emailAddress: data.record.emailAddress,
                                });
                            },
                            visible: abp.auth.isGranted('CmsKit.Newsletter.EditPreferences')
                        }
                    ]
                }
            },
            {
                title: l("EmailAddress"),
                data: "emailAddress"
            },
            {
                title: l("Preferences"),
                data: "preferences",
                render: function (data) {
                    return data.join(", ");
                }
            },
            {
                title: l("CreationTime"),
                data: "creationTime",
                dataFormat: "datetime",
            }
        ]
    }));

    $("#NewslettersTable").data("dataTable", dataTable);

    detailModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });
    
    editPreferencesModal.onResult(function () {
        dataTable.ajax.reloadEx();
    });

    $("#ExportCsv").on("click", "", function () {
        var preference = $("#Preference").val();
        var source = $("#Source").val();
        var emailAddress = $("#EmailAddress").val();
        newsletterService.getDownloadToken().then(
            function(result){
                var url =  abp.appPath + 'api/cms-kit-admin/newsletter/export-csv' +
                    abp.utils.buildQueryString([
                        { name: 'Token', value: result.token },
                        { name: 'Preference', value: preference },
                        { name: 'Source', value: source },
                        { name: 'EmailAddress', value: emailAddress }
                    ]);

                var downloadWindow = window.open(url, '_blank');
                downloadWindow.focus();
            }
        )
    });

    $("#SearchButton").on("click", "", function () {
        dataTable.ajax.reloadEx();
    });

    $("#Source").keypress(function (event) {
        let keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == "13") {
            dataTable.ajax.reloadEx();
        }
    })

    $("#PreferenceFilter").on("change", "", function () {
        dataTable.ajax.reloadEx();
    });

    $('#NewslettersTable').on('draw.dt', function () {
        let info = dataTable.page.info();
        let exportCsv = $("#ExportCsv");

        if (info.recordsDisplay === 0) {
            exportCsv.hide();
        } else if (info.recordsDisplay > 0) {
            exportCsv.show();
        }
    });
});
