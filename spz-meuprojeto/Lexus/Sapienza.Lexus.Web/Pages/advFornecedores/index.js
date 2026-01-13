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

    $("#advFornecedoresFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advFornecedoresFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advFornecedoresFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advFornecedores/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advFornecedores/EditModal');

    var dataTable = $('#advFornecedoresTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advFornecedores.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advFornecedores.Delete'),
                            confirmMessage: function (data) {
                                return l('advFornecedoresDeletionConfirmationMessage', data.record.id);
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
                title: l('advFornecedores:idFornecedor'),
                data: "idFornecedor",
            },
            {
                title: l('advFornecedores:apelido'),
                data: "apelido",
            },
            {
                title: l('advFornecedores:nome'),
                data: "nome",
            },
            {
                title: l('advFornecedores:email'),
                data: "email",
            },
            {
                title: l('advFornecedores:telCelular'),
                data: "telCelular",
            },
            {
                title: l('advFornecedores:telCelularObs'),
                data: "telCelularObs",
            },
            {
                title: l('advFornecedores:telFixo'),
                data: "telFixo",
            },
            {
                title: l('advFornecedores:telFixoObs'),
                data: "telFixoObs",
            },
            {
                title: l('advFornecedores:endereco'),
                data: "endereco",
            },
            {
                title: l('advFornecedores:numero'),
                data: "numero",
            },
            {
                title: l('advFornecedores:complemento'),
                data: "complemento",
            },
            {
                title: l('advFornecedores:bairro'),
                data: "bairro",
            },
            {
                title: l('advFornecedores:cep'),
                data: "cep",
            },
            {
                title: l('advFornecedores:estado'),
                data: "estado",
            },
            {
                title: l('advFornecedores:cidade'),
                data: "cidade",
            },
            {
                title: l('advFornecedores:observacoes'),
                data: "observacoes",
            },
            {
                title: l('advFornecedores:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advFornecedores:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advFornecedores:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advFornecedores:parceiroEmProcesso'),
                data: "parceiroEmProcesso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advFornecedores:parceiroEmProcessoPerc'),
                data: "parceiroEmProcessoPerc",
            },
            {
                title: l('advFornecedores:idProfissional'),
                data: "idProfissional",
            },
            {
                title: l('advFornecedores:foto'),
                data: "foto",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvFornecedoresButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
