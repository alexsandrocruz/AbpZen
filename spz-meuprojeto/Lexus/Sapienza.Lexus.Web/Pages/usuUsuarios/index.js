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

    $("#usuUsuariosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#usuUsuariosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/usuUsuariosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'usuUsuarios/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'usuUsuarios/EditModal');

    var dataTable = $('#usuUsuariosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.usuUsuarios.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.usuUsuarios.Delete'),
                            confirmMessage: function (data) {
                                return l('usuUsuariosDeletionConfirmationMessage', data.record.id);
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
                title: l('usuUsuarios:idUsuario'),
                data: "idUsuario",
            },
            {
                title: l('usuUsuarios:nome'),
                data: "nome",
            },
            {
                title: l('usuUsuarios:sobrenome'),
                data: "sobrenome",
            },
            {
                title: l('usuUsuarios:idArea'),
                data: "idArea",
            },
            {
                title: l('usuUsuarios:idCargo'),
                data: "idCargo",
            },
            {
                title: l('usuUsuarios:login'),
                data: "login",
            },
            {
                title: l('usuUsuarios:senha'),
                data: "senha",
            },
            {
                title: l('usuUsuarios:diaNascimento'),
                data: "diaNascimento",
            },
            {
                title: l('usuUsuarios:mesNascimento'),
                data: "mesNascimento",
            },
            {
                title: l('usuUsuarios:anoNascimento'),
                data: "anoNascimento",
            },
            {
                title: l('usuUsuarios:email'),
                data: "email",
            },
            {
                title: l('usuUsuarios:telCelular'),
                data: "telCelular",
            },
            {
                title: l('usuUsuarios:telFixo'),
                data: "telFixo",
            },
            {
                title: l('usuUsuarios:endereco'),
                data: "endereco",
            },
            {
                title: l('usuUsuarios:numero'),
                data: "numero",
            },
            {
                title: l('usuUsuarios:complemento'),
                data: "complemento",
            },
            {
                title: l('usuUsuarios:bairro'),
                data: "bairro",
            },
            {
                title: l('usuUsuarios:cep'),
                data: "cep",
            },
            {
                title: l('usuUsuarios:estado'),
                data: "estado",
            },
            {
                title: l('usuUsuarios:cidade'),
                data: "cidade",
            },
            {
                title: l('usuUsuarios:cpf'),
                data: "cpf",
            },
            {
                title: l('usuUsuarios:banco'),
                data: "banco",
            },
            {
                title: l('usuUsuarios:agencia'),
                data: "agencia",
            },
            {
                title: l('usuUsuarios:conta'),
                data: "conta",
            },
            {
                title: l('usuUsuarios:foto'),
                data: "foto",
            },
            {
                title: l('usuUsuarios:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('usuUsuarios:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('usuUsuarios:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('usuUsuarios:cor'),
                data: "cor",
            },
            {
                title: l('usuUsuarios:dashboardInicial'),
                data: "dashboardInicial",
            },
            {
                title: l('usuUsuarios:tokenPhoneApp'),
                data: "tokenPhoneApp",
            },
            {
                title: l('usuUsuarios:estadoCivil'),
                data: "estadoCivil",
            },
            {
                title: l('usuUsuarios:nrFilhos'),
                data: "nrFilhos",
            },
            {
                title: l('usuUsuarios:idadeFilhoMenor'),
                data: "idadeFilhoMenor",
            },
            {
                title: l('usuUsuarios:formacaoAcademica'),
                data: "formacaoAcademica",
            },
            {
                title: l('usuUsuarios:regiao'),
                data: "regiao",
            },
            {
                title: l('usuUsuarios:idSuperior'),
                data: "idSuperior",
            },
            {
                title: l('usuUsuarios:master'),
                data: "master",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('usuUsuarios:mediaConsumoLitro'),
                data: "mediaConsumoLitro",
            },
            {
                title: l('usuUsuarios:distanciasIguais'),
                data: "distanciasIguais",
            },
            {
                title: l('usuUsuarios:chaveChamados'),
                data: "chaveChamados",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewusuUsuariosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
