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

    $("#advClientesFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advClientesFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advClientesFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advClientes/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advClientes/EditModal');

    var dataTable = $('#advClientesTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientes.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientes.Delete'),
                            confirmMessage: function (data) {
                                return l('advClientesDeletionConfirmationMessage', data.record.id);
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
                title: l('advClientes:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advClientes:apelido'),
                data: "apelido",
            },
            {
                title: l('advClientes:idGrupo'),
                data: "idGrupo",
            },
            {
                title: l('advClientes:idSituacao'),
                data: "idSituacao",
            },
            {
                title: l('advClientes:nome'),
                data: "nome",
            },
            {
                title: l('advClientes:email'),
                data: "email",
            },
            {
                title: l('advClientes:telCelular'),
                data: "telCelular",
            },
            {
                title: l('advClientes:telCelularObs'),
                data: "telCelularObs",
            },
            {
                title: l('advClientes:telFixo'),
                data: "telFixo",
            },
            {
                title: l('advClientes:telFixoObs'),
                data: "telFixoObs",
            },
            {
                title: l('advClientes:dataNascimento'),
                data: "dataNascimento",
            },
            {
                title: l('advClientes:cpf'),
                data: "cpf",
            },
            {
                title: l('advClientes:rg'),
                data: "rg",
            },
            {
                title: l('advClientes:ctps'),
                data: "ctps",
            },
            {
                title: l('advClientes:endereco'),
                data: "endereco",
            },
            {
                title: l('advClientes:numero'),
                data: "numero",
            },
            {
                title: l('advClientes:complemento'),
                data: "complemento",
            },
            {
                title: l('advClientes:bairro'),
                data: "bairro",
            },
            {
                title: l('advClientes:cep'),
                data: "cep",
            },
            {
                title: l('advClientes:estado'),
                data: "estado",
            },
            {
                title: l('advClientes:cidade'),
                data: "cidade",
            },
            {
                title: l('advClientes:dataIngresso'),
                data: "dataIngresso",
            },
            {
                title: l('advClientes:observacoes'),
                data: "observacoes",
            },
            {
                title: l('advClientes:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes:naturalEstado'),
                data: "naturalEstado",
            },
            {
                title: l('advClientes:naturalCidade'),
                data: "naturalCidade",
            },
            {
                title: l('advClientes:nomeDaMae'),
                data: "nomeDaMae",
            },
            {
                title: l('advClientes:dib'),
                data: "dib",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:dibData'),
                data: "dibData",
            },
            {
                title: l('advClientes:dibIdTipoBeneficio'),
                data: "dibIdTipoBeneficio",
            },
            {
                title: l('advClientes:idCargo'),
                data: "idCargo",
            },
            {
                title: l('advClientes:telCelular2'),
                data: "telCelular2",
            },
            {
                title: l('advClientes:telCelular2Obs'),
                data: "telCelular2Obs",
            },
            {
                title: l('advClientes:telFixo2'),
                data: "telFixo2",
            },
            {
                title: l('advClientes:telFixo2Obs'),
                data: "telFixo2Obs",
            },
            {
                title: l('advClientes:cnpj'),
                data: "cnpj",
            },
            {
                title: l('advClientes:ie'),
                data: "ie",
            },
            {
                title: l('advClientes:idFornecedor'),
                data: "idFornecedor",
            },
            {
                title: l('advClientes:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advClientes:inssAgendado'),
                data: "inssAgendado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:inssData'),
                data: "inssData",
            },
            {
                title: l('advClientes:inssIdTipoBeneficio'),
                data: "inssIdTipoBeneficio",
            },
            {
                title: l('advClientes:inssIdPosto'),
                data: "inssIdPosto",
            },
            {
                title: l('advClientes:inssResultado'),
                data: "inssResultado",
            },
            {
                title: l('advClientes:prospect'),
                data: "prospect",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:idLocalAtendido'),
                data: "idLocalAtendido",
            },
            {
                title: l('advClientes:whatsapp'),
                data: "whatsapp",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:pastaFTP'),
                data: "pastaFTP",
            },
            {
                title: l('advClientes:inssResponsavel'),
                data: "inssResponsavel",
            },
            {
                title: l('advClientes:responsavelPendencia'),
                data: "responsavelPendencia",
            },
            {
                title: l('advClientes:comoChegou'),
                data: "comoChegou",
            },
            {
                title: l('advClientes:inssProtocolo'),
                data: "inssProtocolo",
            },
            {
                title: l('advClientes:inssTsInclusao'),
                data: "inssTsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes:inssIdUsuarioInclusao'),
                data: "inssIdUsuarioInclusao",
            },
            {
                title: l('advClientes:foto'),
                data: "foto",
            },
            {
                title: l('advClientes:followBloqueadoAte'),
                data: "followBloqueadoAte",
            },
            {
                title: l('advClientes:falecido'),
                data: "falecido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:senhaINSSDigital'),
                data: "senhaINSSDigital",
            },
            {
                title: l('advClientes:idPrioridade'),
                data: "idPrioridade",
            },
            {
                title: l('advClientes:instagram'),
                data: "instagram",
            },
            {
                title: l('advClientes:rgOrgaoExp'),
                data: "rgOrgaoExp",
            },
            {
                title: l('advClientes:nacionalidade'),
                data: "nacionalidade",
            },
            {
                title: l('advClientes:estadocivil'),
                data: "estadocivil",
            },
            {
                title: l('advClientes:dcb'),
                data: "dcb",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes:dcbData'),
                data: "dcbData",
            },
            {
                title: l('advClientes:finIdUnidade'),
                data: "finIdUnidade",
            },
            {
                title: l('advClientes:finIdCentroCusto'),
                data: "finIdCentroCusto",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvClientesButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
