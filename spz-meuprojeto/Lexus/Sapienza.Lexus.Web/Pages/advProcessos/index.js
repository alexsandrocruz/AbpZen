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

    $("#advProcessosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advProcessosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advProcessosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advProcessos/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advProcessos/EditModal');

    var dataTable = $('#advProcessosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessos.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessos.Delete'),
                            confirmMessage: function (data) {
                                return l('advProcessosDeletionConfirmationMessage', data.record.id);
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
                title: l('advProcessos:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advProcessos:idCliente'),
                data: "idCliente",
            },
            {
                title: l('advProcessos:idUsuarioInclusao'),
                data: "idUsuarioInclusao",
            },
            {
                title: l('advProcessos:idEscritorioOrigem'),
                data: "idEscritorioOrigem",
            },
            {
                title: l('advProcessos:idEscritorioResponsavel'),
                data: "idEscritorioResponsavel",
            },
            {
                title: l('advProcessos:idAutorPeticao'),
                data: "idAutorPeticao",
            },
            {
                title: l('advProcessos:idResponsavel'),
                data: "idResponsavel",
            },
            {
                title: l('advProcessos:sintese'),
                data: "sintese",
            },
            {
                title: l('advProcessos:numero'),
                data: "numero",
            },
            {
                title: l('advProcessos:dataDistribuicao'),
                data: "dataDistribuicao",
            },
            {
                title: l('advProcessos:idStatus'),
                data: "idStatus",
            },
            {
                title: l('advProcessos:idNatureza'),
                data: "idNatureza",
            },
            {
                title: l('advProcessos:idTipo'),
                data: "idTipo",
            },
            {
                title: l('advProcessos:estado'),
                data: "estado",
            },
            {
                title: l('advProcessos:cidade'),
                data: "cidade",
            },
            {
                title: l('advProcessos:idFase'),
                data: "idFase",
            },
            {
                title: l('advProcessos:idRelevancia'),
                data: "idRelevancia",
            },
            {
                title: l('advProcessos:idProbabilidade'),
                data: "idProbabilidade",
            },
            {
                title: l('advProcessos:valorCausa'),
                data: "valorCausa",
            },
            {
                title: l('advProcessos:valorHonorarios'),
                data: "valorHonorarios",
            },
            {
                title: l('advProcessos:valorHonorariosTipo'),
                data: "valorHonorariosTipo",
            },
            {
                title: l('advProcessos:observacoes'),
                data: "observacoes",
            },
            {
                title: l('advProcessos:idSentenca'),
                data: "idSentenca",
            },
            {
                title: l('advProcessos:dataSentenca'),
                data: "dataSentenca",
            },
            {
                title: l('advProcessos:alvara'),
                data: "alvara",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:valorDeferido'),
                data: "valorDeferido",
            },
            {
                title: l('advProcessos:dataEncerramento'),
                data: "dataEncerramento",
            },
            {
                title: l('advProcessos:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessos:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessos:idOrgao'),
                data: "idOrgao",
            },
            {
                title: l('advProcessos:idInstancia'),
                data: "idInstancia",
            },
            {
                title: l('advProcessos:idVara'),
                data: "idVara",
            },
            {
                title: l('advProcessos:recurso'),
                data: "recurso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:recursoIdSentenca'),
                data: "recursoIdSentenca",
            },
            {
                title: l('advProcessos:recursoDataSentenca'),
                data: "recursoDataSentenca",
            },
            {
                title: l('advProcessos:alvaraPendente'),
                data: "alvaraPendente",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:alvaraPendenteDesde'),
                data: "alvaraPendenteDesde",
            },
            {
                title: l('advProcessos:historicoNumeros'),
                data: "historicoNumeros",
            },
            {
                title: l('advProcessos:recebeAcordo'),
                data: "recebeAcordo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:recebeRPV'),
                data: "recebeRPV",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:recebePrecatorio'),
                data: "recebePrecatorio",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:recebeAlvara'),
                data: "recebeAlvara",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:recebeBanco'),
                data: "recebeBanco",
            },
            {
                title: l('advProcessos:recebeDataLiberacao'),
                data: "recebeDataLiberacao",
            },
            {
                title: l('advProcessos:pendOutrosValores'),
                data: "pendOutrosValores",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:pendOutrosValoresDataEncerramento'),
                data: "pendOutrosValoresDataEncerramento",
            },
            {
                title: l('advProcessos:pendOutrosValoresDeferido'),
                data: "pendOutrosValoresDeferido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:pendOutrosValoresValorDeferido'),
                data: "pendOutrosValoresValorDeferido",
            },
            {
                title: l('advProcessos:acaoColetiva'),
                data: "acaoColetiva",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:temResponsavel'),
                data: "temResponsavel",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:nomeResponsavel'),
                data: "nomeResponsavel",
            },
            {
                title: l('advProcessos:cpfResponsavel'),
                data: "cpfResponsavel",
            },
            {
                title: l('advProcessos:imposto'),
                data: "imposto",
            },
            {
                title: l('advProcessos:tarifa'),
                data: "tarifa",
            },
            {
                title: l('advProcessos:complementoPositivo'),
                data: "complementoPositivo",
            },
            {
                title: l('advProcessos:RPV'),
                data: "rPV",
            },
            {
                title: l('advProcessos:bancarioBanco'),
                data: "bancarioBanco",
            },
            {
                title: l('advProcessos:bancarioTipoConta'),
                data: "bancarioTipoConta",
            },
            {
                title: l('advProcessos:bancarioAgencia'),
                data: "bancarioAgencia",
            },
            {
                title: l('advProcessos:bancarioConta'),
                data: "bancarioConta",
            },
            {
                title: l('advProcessos:bancarioFavorecido'),
                data: "bancarioFavorecido",
            },
            {
                title: l('advProcessos:bancarioCpf'),
                data: "bancarioCpf",
            },
            {
                title: l('advProcessos:nomeReu'),
                data: "nomeReu",
            },
            {
                title: l('advProcessos:sucumbencia'),
                data: "sucumbencia",
            },
            {
                title: l('advProcessos:idConta'),
                data: "idConta",
            },
            {
                title: l('advProcessos:dataLiberacaoValorDeferido'),
                data: "dataLiberacaoValorDeferido",
            },
            {
                title: l('advProcessos:boleto'),
                data: "boleto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:precatorio'),
                data: "precatorio",
            },
            {
                title: l('advProcessos:emitir'),
                data: "emitir",
            },
            {
                title: l('advProcessos:emitido'),
                data: "emitido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:formaRecebimento'),
                data: "formaRecebimento",
            },
            {
                title: l('advProcessos:bancarioBancoId'),
                data: "bancarioBancoId",
            },
            {
                title: l('advProcessos:dataPrevisaoRepasseCliente'),
                data: "dataPrevisaoRepasseCliente",
            },
            {
                title: l('advProcessos:honorariosTextoFicha'),
                data: "honorariosTextoFicha",
            },
            {
                title: l('advProcessos:nfComComplementoPositivo'),
                data: "nfComComplementoPositivo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:valorHonorariosDestaque'),
                data: "valorHonorariosDestaque",
            },
            {
                title: l('advProcessos:valorHonorariosDestaqueTipo'),
                data: "valorHonorariosDestaqueTipo",
            },
            {
                title: l('advProcessos:dataPrevisaoHonorariosDestaque'),
                data: "dataPrevisaoHonorariosDestaque",
            },
            {
                title: l('advProcessos:idContaPagar'),
                data: "idContaPagar",
            },
            {
                title: l('advProcessos:bancarioPerc'),
                data: "bancarioPerc",
            },
            {
                title: l('advProcessos:dataPrevistaClienteReceber'),
                data: "dataPrevistaClienteReceber",
            },
            {
                title: l('advProcessos:sucumbenciaAdd'),
                data: "sucumbenciaAdd",
            },
            {
                title: l('advProcessos:sucumbenciaAddData'),
                data: "sucumbenciaAddData",
            },
            {
                title: l('advProcessos:sucumbenciaAddIdBanco'),
                data: "sucumbenciaAddIdBanco",
            },
            {
                title: l('advProcessos:saldoDevedor'),
                data: "saldoDevedor",
            },
            {
                title: l('advProcessos:herdeirosTipoValor'),
                data: "herdeirosTipoValor",
            },
            {
                title: l('advProcessos:nrParcelasProcesso'),
                data: "nrParcelasProcesso",
            },
            {
                title: l('advProcessos:nrParcelasSomenteSucumbencia'),
                data: "nrParcelasSomenteSucumbencia",
            },
            {
                title: l('advProcessos:preProcesso'),
                data: "preProcesso",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:preProcessoPasta'),
                data: "preProcessoPasta",
            },
            {
                title: l('advProcessos:preProcessoDataCriacao'),
                data: "preProcessoDataCriacao",
            },
            {
                title: l('advProcessos:preProcessoDataPrevista'),
                data: "preProcessoDataPrevista",
            },
            {
                title: l('advProcessos:preProcessoDataRealizada'),
                data: "preProcessoDataRealizada",
            },
            {
                title: l('advProcessos:preProcessoIdStatus'),
                data: "preProcessoIdStatus",
            },
            {
                title: l('advProcessos:tsConversao'),
                data: "tsConversao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessos:perdido'),
                data: "perdido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:tsPerdido'),
                data: "tsPerdido",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessos:idMotivoPerda'),
                data: "idMotivoPerda",
            },
            {
                title: l('advProcessos:convertido'),
                data: "convertido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:clientePrimeiraVez'),
                data: "clientePrimeiraVez",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessos:preProcessoIdTipo'),
                data: "preProcessoIdTipo",
            },
            {
                title: l('advProcessos:tarifaParcelas'),
                data: "tarifaParcelas",
            },
            {
                title: l('advProcessos:idOrigem'),
                data: "idOrigem",
            },
            {
                title: l('advProcessos:dataEntrada'),
                data: "dataEntrada",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvProcessosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
