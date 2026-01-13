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

    $("#advClientes_bkpFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advClientes_bkpFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advClientes_bkpFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advClientes_bkp/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advClientes_bkp/EditModal');

    var dataTable = $('#advClientes_bkpTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientes_bkp.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advClientes_bkp.Delete'),
                            confirmMessage: function (data) {
                                return l('advClientes_bkpDeletionConfirmationMessage', data.record.id);
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
                title: l('advClientes_bkp:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advClientes_bkp:apelido'),
                data: "apelido",
            },
            {
                title: l('advClientes_bkp:idGrupo'),
                data: "idGrupo",
            },
            {
                title: l('advClientes_bkp:idSituacao'),
                data: "idSituacao",
            },
            {
                title: l('advClientes_bkp:nome'),
                data: "nome",
            },
            {
                title: l('advClientes_bkp:email'),
                data: "email",
            },
            {
                title: l('advClientes_bkp:telCelular'),
                data: "telCelular",
            },
            {
                title: l('advClientes_bkp:telCelularObs'),
                data: "telCelularObs",
            },
            {
                title: l('advClientes_bkp:telFixo'),
                data: "telFixo",
            },
            {
                title: l('advClientes_bkp:telFixoObs'),
                data: "telFixoObs",
            },
            {
                title: l('advClientes_bkp:dataNascimento'),
                data: "dataNascimento",
            },
            {
                title: l('advClientes_bkp:cpf'),
                data: "cpf",
            },
            {
                title: l('advClientes_bkp:rg'),
                data: "rg",
            },
            {
                title: l('advClientes_bkp:ctps'),
                data: "ctps",
            },
            {
                title: l('advClientes_bkp:endereco'),
                data: "endereco",
            },
            {
                title: l('advClientes_bkp:numero'),
                data: "numero",
            },
            {
                title: l('advClientes_bkp:complemento'),
                data: "complemento",
            },
            {
                title: l('advClientes_bkp:bairro'),
                data: "bairro",
            },
            {
                title: l('advClientes_bkp:cep'),
                data: "cep",
            },
            {
                title: l('advClientes_bkp:estado'),
                data: "estado",
            },
            {
                title: l('advClientes_bkp:cidade'),
                data: "cidade",
            },
            {
                title: l('advClientes_bkp:dataIngresso'),
                data: "dataIngresso",
            },
            {
                title: l('advClientes_bkp:observacoes'),
                data: "observacoes",
            },
            {
                title: l('advClientes_bkp:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes_bkp:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes_bkp:naturalEstado'),
                data: "naturalEstado",
            },
            {
                title: l('advClientes_bkp:naturalCidade'),
                data: "naturalCidade",
            },
            {
                title: l('advClientes_bkp:nomeDaMae'),
                data: "nomeDaMae",
            },
            {
                title: l('advClientes_bkp:dib'),
                data: "dib",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:dibData'),
                data: "dibData",
            },
            {
                title: l('advClientes_bkp:dibIdTipoBeneficio'),
                data: "dibIdTipoBeneficio",
            },
            {
                title: l('advClientes_bkp:idCargo'),
                data: "idCargo",
            },
            {
                title: l('advClientes_bkp:telCelular2'),
                data: "telCelular2",
            },
            {
                title: l('advClientes_bkp:telCelular2Obs'),
                data: "telCelular2Obs",
            },
            {
                title: l('advClientes_bkp:telFixo2'),
                data: "telFixo2",
            },
            {
                title: l('advClientes_bkp:telFixo2Obs'),
                data: "telFixo2Obs",
            },
            {
                title: l('advClientes_bkp:cnpj'),
                data: "cnpj",
            },
            {
                title: l('advClientes_bkp:ie'),
                data: "ie",
            },
            {
                title: l('advClientes_bkp:idFornecedor'),
                data: "idFornecedor",
            },
            {
                title: l('advClientes_bkp:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advClientes_bkp:inssAgendado'),
                data: "inssAgendado",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:inssData'),
                data: "inssData",
            },
            {
                title: l('advClientes_bkp:inssIdTipoBeneficio'),
                data: "inssIdTipoBeneficio",
            },
            {
                title: l('advClientes_bkp:inssIdPosto'),
                data: "inssIdPosto",
            },
            {
                title: l('advClientes_bkp:inssResultado'),
                data: "inssResultado",
            },
            {
                title: l('advClientes_bkp:prospect'),
                data: "prospect",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:idLocalAtendido'),
                data: "idLocalAtendido",
            },
            {
                title: l('advClientes_bkp:whatsapp'),
                data: "whatsapp",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:pastaFTP'),
                data: "pastaFTP",
            },
            {
                title: l('advClientes_bkp:inssResponsavel'),
                data: "inssResponsavel",
            },
            {
                title: l('advClientes_bkp:responsavelPendencia'),
                data: "responsavelPendencia",
            },
            {
                title: l('advClientes_bkp:comoChegou'),
                data: "comoChegou",
            },
            {
                title: l('advClientes_bkp:inssProtocolo'),
                data: "inssProtocolo",
            },
            {
                title: l('advClientes_bkp:inssTsInclusao'),
                data: "inssTsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advClientes_bkp:inssIdUsuarioInclusao'),
                data: "inssIdUsuarioInclusao",
            },
            {
                title: l('advClientes_bkp:foto'),
                data: "foto",
            },
            {
                title: l('advClientes_bkp:followBloqueadoAte'),
                data: "followBloqueadoAte",
            },
            {
                title: l('advClientes_bkp:falecido'),
                data: "falecido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:senhaINSSDigital'),
                data: "senhaINSSDigital",
            },
            {
                title: l('advClientes_bkp:idPrioridade'),
                data: "idPrioridade",
            },
            {
                title: l('advClientes_bkp:instagram'),
                data: "instagram",
            },
            {
                title: l('advClientes_bkp:rgOrgaoExp'),
                data: "rgOrgaoExp",
            },
            {
                title: l('advClientes_bkp:nacionalidade'),
                data: "nacionalidade",
            },
            {
                title: l('advClientes_bkp:estadocivil'),
                data: "estadocivil",
            },
            {
                title: l('advClientes_bkp:dcb'),
                data: "dcb",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advClientes_bkp:dcbData'),
                data: "dcbData",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvClientes_bkpButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
