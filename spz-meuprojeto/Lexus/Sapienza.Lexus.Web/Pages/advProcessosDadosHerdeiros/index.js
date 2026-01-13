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

    $("#advProcessosDadosHerdeirosFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#advProcessosDadosHerdeirosFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/advProcessosDadosHerdeirosFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'advProcessosDadosHerdeiros/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'advProcessosDadosHerdeiros/EditModal');

    var dataTable = $('#advProcessosDadosHerdeirosTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessosDadosHerdeiros.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.advProcessosDadosHerdeiros.Delete'),
                            confirmMessage: function (data) {
                                return l('advProcessosDadosHerdeirosDeletionConfirmationMessage', data.record.id);
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
                title: l('advProcessosDadosHerdeiros:idHerdeiro'),
                data: "idHerdeiro",
            },
            {
                title: l('advProcessosDadosHerdeiros:idProcesso'),
                data: "idProcesso",
            },
            {
                title: l('advProcessosDadosHerdeiros:sequencia'),
                data: "sequencia",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioBancoId'),
                data: "bancarioBancoId",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioTipoConta'),
                data: "bancarioTipoConta",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioAgencia'),
                data: "bancarioAgencia",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioConta'),
                data: "bancarioConta",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioFavorecido'),
                data: "bancarioFavorecido",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioCpf'),
                data: "bancarioCpf",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioPerc'),
                data: "bancarioPerc",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioTarifa'),
                data: "bancarioTarifa",
            },
            {
                title: l('advProcessosDadosHerdeiros:bancarioTarifaParcelas'),
                data: "bancarioTarifaParcelas",
            },
            {
                title: l('advProcessosDadosHerdeiros:idHonorario'),
                data: "idHonorario",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewadvProcessosDadosHerdeirosButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
