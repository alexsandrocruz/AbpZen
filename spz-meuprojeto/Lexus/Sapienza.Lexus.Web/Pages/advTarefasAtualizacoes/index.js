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

    $("#advTarefasAtualizacoesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advTarefasAtualizacoesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advTarefasAtualizacoesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advTarefasAtualizacoes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advTarefasAtualizacoes/EditModal');

    var dataTable = $('#advTarefasAtualizacoesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advTarefasAtualizacoes.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advTarefasAtualizacoes.Delete'),
                            confirmMessage: function (data) {
                                return l('advTarefasAtualizacoesDeletionConfirmationMessage', data.record.id);
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
                title: l('advTarefasAtualizacoes:idAtualizacaoTarefa'),
                data: "idAtualizacaoTarefa",
            },
            {
                title: l('advTarefasAtualizacoes:idTarefa'),
                data: "idTarefa",
            },
            {
                title: l('advTarefasAtualizacoes:idCompromisso'),
                data: "idCompromisso",
            },
            {
                title: l('advTarefasAtualizacoes:campo'),
                data: "campo",
            },
            {
                title: l('advTarefasAtualizacoes:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advTarefasAtualizacoes:dadoAnterior'),
                data: "dadoAnterior",
            },
            {
                title: l('advTarefasAtualizacoes:idUsuario'),
                data: "idUsuario",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvTarefasAtualizacoesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
