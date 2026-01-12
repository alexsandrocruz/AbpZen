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

    $("#AlunoTurmaFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#AlunoTurmaFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/AlunoTurmaFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'AlunoTurma/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'AlunoTurma/EditModal');

    var dataTable = $('#AlunoTurmaTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('LeptonXDemoApp.AlunoTurma.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.AlunoTurma.Delete'),
                            confirmMessage: function (data) {
                                return l('AlunoTurmaDeletionConfirmationMessage', data.record.id);
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
                title: l('AlunoTurma:DataMatricula'),
                data: "dataMatricula",
                dataFormat: 'datetime'
            },
            {
                title: l('AlunoTurma:Situacao'),
                data: "situacao",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewAlunoTurmaButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
