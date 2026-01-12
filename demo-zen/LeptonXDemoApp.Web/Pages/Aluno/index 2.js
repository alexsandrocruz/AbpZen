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

    $("#AlunoFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#AlunoFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/AlunoFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('LeptonXDemoApp');
    // Master-Detail: Full Page CRUD

    var dataTable = $('#AlunoTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('LeptonXDemoApp.Aluno.Update'),
                            action: function (data) {
                                window.location.href = '/Aluno/Edit/' + data.record.id;
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('LeptonXDemoApp.Aluno.Delete'),
                            confirmMessage: function (data) {
                                return l('AlunoDeletionConfirmationMessage', data.record.id);
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
                title: l('Aluno:Nome'),
                data: "nome",
            },
            {
                title: l('Aluno:Email'),
                data: "email",
            },
            {
                title: l('Aluno:Matricula'),
                data: "matricula",
            },
            {
                title: l('Aluno:DataNascimento'),
                data: "dataNascimento",
                dataFormat: 'datetime'
            },
        ]
    }));

    $('#NewAlunoButton').click(function (e) {
        e.preventDefault();
        window.location.href = '/Aluno/Create';
    });
});
