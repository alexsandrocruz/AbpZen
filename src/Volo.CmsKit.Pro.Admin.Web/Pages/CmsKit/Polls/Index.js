$(function () {
    var l = abp.localization.getResource("CmsKit");

    var pollService = volo.cmsKit.admin.polls.pollAdmin;

    var createModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Polls/CreateModal", modalClass: 'newPollButton' });
    var editModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Polls/EditModal", modalClass: 'editPoll' });
    var resultModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Polls/ResultModal", modalClass: 'resultPoll' });


    var getFilter = function () {
        return {
            filter: $("#Filter").val()
        };
    };

    let dataTable = $("#PollTable").DataTable(abp.libs.datatables.normalizeConfiguration({
        searching: false,
        processing: true,
        scrollX: true,
        serverSide: true,
        paging: true,
        ajax: abp.libs.datatables.createAjax(pollService.getList, getFilter),
        columnDefs: [
            {
                title: l("Actions"),
                targets: 0,
                width: "20%",
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('CmsKit.Poll.Update'),
                            action: function (data) {
                                editModal.open({
                                    id: data.record.id
                                });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('CmsKit.Poll.Delete'),
                            confirmMessage: function (data) {
                                return l("PollDeletionConfirmationMessage", data.record.name)
                            },
                            action: function (data) {
                                pollService.delete(data.record.id)
                                    .then(function () {
                                        dataTable.ajax.reloadEx();
                                        abp.notify.success(l('DeletedSuccessfully'));
                                    });
                            }
                        },
                        {
                            text: l('ShowResults'),
                            visible: abp.auth.isGranted('CmsKit.Poll'),
                            action: function (data) {
                                resultModal.open({
                                    id: data.record.id
                                });
                            }
                        },
                        {
                            text: l('CopyWidgetCode'),
                            visible: abp.auth.isGranted('CmsKit.Poll'),
                            action: function (data) {
                                var sampleTextarea = document.createElement("textarea");
                                document.body.appendChild(sampleTextarea);
                                sampleTextarea.value = "[Widget Type=\"Poll\" Code=\"" + data.record.code + "\"]";
                                sampleTextarea.select();
                                document.execCommand("copy");
                                document.body.removeChild(sampleTextarea);

                                abp.notify.success(l('CopiedWidgetCode'));
                            }
                        },
                    ]
                }
            },
            {
                width: "40%",
                title: l("Question"),
                data: "question"
            },
            {
                width: "20%",
                title: l("Name"),
                data: "name"
            },
            {
                width: "20%",
                title: l("Code"),
                data: "code"
            },
            {
                width: "20%",
                title: l("VoteCount"),
                data: "voteCount"
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

    $('button[name=NewPollButton]').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    $("#Filter").keypress(function (event) {
        let keyCode = (event.keyCode ? event.keyCode : event.which);
        if (keyCode == "13") {
            dataTable.ajax.reloadEx();
        }
    })
});
