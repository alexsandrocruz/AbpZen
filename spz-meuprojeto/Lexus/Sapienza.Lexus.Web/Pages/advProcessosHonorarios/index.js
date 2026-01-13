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

    $("#advProcessosHonorariosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advProcessosHonorariosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advProcessosHonorariosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advProcessosHonorarios/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advProcessosHonorarios/EditModal');

    var dataTable = $('#advProcessosHonorariosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessosHonorarios.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessosHonorarios.Delete'),
                            confirmMessage: function (data) {
                                return l('advProcessosHonorariosDeletionConfirmationMessage', data.record.id);
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
                title: l('advProcessosHonorarios:idHonorario'),
                data: "idHonorario",
            },
            {
                title: l('advProcessosHonorarios:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advProcessosHonorarios:dataPrevistaClienteReceber'),
                data: "dataPrevistaClienteReceber",
            },
            {
                title: l('advProcessosHonorarios:rpv'),
                data: "rpv",
            },
            {
                title: l('advProcessosHonorarios:precatorio'),
                data: "precatorio",
            },
            {
                title: l('advProcessosHonorarios:nrParcelasProcesso'),
                data: "nrParcelasProcesso",
            },
            {
                title: l('advProcessosHonorarios:valorHonorarios'),
                data: "valorHonorarios",
            },
            {
                title: l('advProcessosHonorarios:valorHonorariosTipo'),
                data: "valorHonorariosTipo",
            },
            {
                title: l('advProcessosHonorarios:honorariosTextoFicha'),
                data: "honorariosTextoFicha",
            },
            {
                title: l('advProcessosHonorarios:valorHonorariosDestaque'),
                data: "valorHonorariosDestaque",
            },
            {
                title: l('advProcessosHonorarios:valorHonorariosDestaqueTipo'),
                data: "valorHonorariosDestaqueTipo",
            },
            {
                title: l('advProcessosHonorarios:dataPrevisaoHonorariosDestaque'),
                data: "dataPrevisaoHonorariosDestaque",
            },
            {
                title: l('advProcessosHonorarios:imposto'),
                data: "imposto",
            },
            {
                title: l('advProcessosHonorarios:complementoPositivo'),
                data: "complementoPositivo",
            },
            {
                title: l('advProcessosHonorarios:sucumbencia'),
                data: "sucumbencia",
            },
            {
                title: l('advProcessosHonorarios:saldoDevedor'),
                data: "saldoDevedor",
            },
            {
                title: l('advProcessosHonorarios:valorDeferido'),
                data: "valorDeferido",
            },
            {
                title: l('advProcessosHonorarios:dataLiberacaoValorDeferido'),
                data: "dataLiberacaoValorDeferido",
            },
            {
                title: l('advProcessosHonorarios:idConta'),
                data: "idConta",
            },
            {
                title: l('advProcessosHonorarios:dataPrevisaoRepasseCliente'),
                data: "dataPrevisaoRepasseCliente",
            },
            {
                title: l('advProcessosHonorarios:idContaPagar'),
                data: "idContaPagar",
            },
            {
                title: l('advProcessosHonorarios:formaRecebimento'),
                data: "formaRecebimento",
            },
            {
                title: l('advProcessosHonorarios:nrParcelasSomenteSucumbencia'),
                data: "nrParcelasSomenteSucumbencia",
            },
            {
                title: l('advProcessosHonorarios:sucumbenciaAdd'),
                data: "sucumbenciaAdd",
            },
            {
                title: l('advProcessosHonorarios:sucumbenciaAddData'),
                data: "sucumbenciaAddData",
            },
            {
                title: l('advProcessosHonorarios:sucumbenciaAddIdBanco'),
                data: "sucumbenciaAddIdBanco",
            },
            {
                title: l('advProcessosHonorarios:boleto'),
                data: "boleto",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessosHonorarios:emitir'),
                data: "emitir",
            },
            {
                title: l('advProcessosHonorarios:emitido'),
                data: "emitido",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessosHonorarios:nfComComplementoPositivo'),
                data: "nfComComplementoPositivo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessosHonorarios:bancarioCpf'),
                data: "bancarioCpf",
            },
            {
                title: l('advProcessosHonorarios:herdeirosTipoValor'),
                data: "herdeirosTipoValor",
            },
            {
                title: l('advProcessosHonorarios:bancarioPerc'),
                data: "bancarioPerc",
            },
            {
                title: l('advProcessosHonorarios:tarifa'),
                data: "tarifa",
            },
            {
                title: l('advProcessosHonorarios:tarifaParcelas'),
                data: "tarifaParcelas",
            },
            {
                title: l('advProcessosHonorarios:bancarioFavorecido'),
                data: "bancarioFavorecido",
            },
            {
                title: l('advProcessosHonorarios:bancarioBancoId'),
                data: "bancarioBancoId",
            },
            {
                title: l('advProcessosHonorarios:bancarioTipoConta'),
                data: "bancarioTipoConta",
            },
            {
                title: l('advProcessosHonorarios:bancarioAgencia'),
                data: "bancarioAgencia",
            },
            {
                title: l('advProcessosHonorarios:bancarioConta'),
                data: "bancarioConta",
            },
            {
                title: l('advProcessosHonorarios:ativo'),
                data: "ativo",
                render: function (data) { return data ? l('Yes') : l('No'); }
            },
            {
                title: l('advProcessosHonorarios:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessosHonorarios:incluidoPor'),
                data: "incluidoPor",
            },
            {
                title: l('advProcessosHonorarios:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('advProcessosHonorarios:alteradoPor'),
                data: "alteradoPor",
            },
            {
                title: l('advProcessosHonorarios:valorHonorariosDestaqueSomente'),
                data: "valorHonorariosDestaqueSomente",
            },
            {
                title: l('advProcessosHonorarios:valorHonorariosDestaqueTipoSomente'),
                data: "valorHonorariosDestaqueTipoSomente",
            },
            {
                title: l('advProcessosHonorarios:dataPrevisaoHonorariosDestaqueSomente'),
                data: "dataPrevisaoHonorariosDestaqueSomente",
            },
            {
                title: l('advProcessosHonorarios:idBancoDestaqueSomente'),
                data: "idBancoDestaqueSomente",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvProcessosHonorariosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
