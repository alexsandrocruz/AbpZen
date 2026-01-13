$(function () {

    function debounce(func, delay) {
        let timerId;
        return function(...args) {
            clearTimeout(timerId);
            timerId = setTimeout(() => {
                func.apply(this, args);
            }, delay);
        };
    }

    $("#advPreProcessosCheckListsFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advPreProcessosCheckListsFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advPreProcessosCheckListsFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advPreProcessosCheckLists/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advPreProcessosCheckLists/EditModal');

    var dataTable = $('#advPreProcessosCheckListsTable').DataTable(abp.libs.datatables.normalizeConfiguration({
        processing: true,
        serverSide: true,
        paging: true,
        searching: false,
        autoWidth: false,
        scrollCollapse: true,
        order: [[0, "asc"]],
        ajax: abp.libs.datatables.createAjax(function (input) {
            return abp.ajax({
                url: '?handler=List',
                type: 'GET',
                data: input
            });
        }, getFilter),
        columnDefs: [
            {
                rowAction: {
                    items: [
                        {
                            text: l('Edit'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advPreProcessosCheckLists.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advPreProcessosCheckLists.Delete'),
                            confirmMessage: function (data) {
                                return l('advPreProcessosCheckListsDeletionConfirmationMessage', data.record.id);
                            },
                            action: function (data) {
                                abp.ajax({
                                    url: '?handler=Delete&id=' + data.record.id,
                                    type: 'POST'
                                })
                                .then(function () {
                                    abp.notify.info(l('SuccessfullyDeleted'));
                                    dataTable.ajax.reload(null, false);
                                });
                            }
                        }
                    ]
                }
            },
            {
                title: l('advPreProcessosCheckLists:idPreCheckList'),
                data: "idPreCheckList",
            },
            {
                title: l('advPreProcessosCheckLists:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advPreProcessosCheckLists:idGrupo'),
                data: "idGrupo",
            },
            {
                title: l('advPreProcessosCheckLists:idCheckList'),
                data: "idCheckList",
            },
            {
                title: l('advPreProcessosCheckLists:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advPreProcessosCheckLists:grupo'),
                data: "grupo",
            },
            {
                title: l('advPreProcessosCheckLists:item'),
                data: "item",
            },
            {
                title: l('advPreProcessosCheckLists:concluido'),
                data: "concluido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advPreProcessosCheckLists:tsConclusao'),
                data: "tsConclusao",
            },
            {
                title: l('advPreProcessosCheckLists:idResponsavel'),
                data: "idResponsavel",
            },
            {
                title: l('advPreProcessosCheckLists:ordem'),
                data: "ordem",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvPreProcessosCheckListsButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
