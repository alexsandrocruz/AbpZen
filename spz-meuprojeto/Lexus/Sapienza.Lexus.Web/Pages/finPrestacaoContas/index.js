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

    $("#finPrestacaoContasFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#finPrestacaoContasFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/finPrestacaoContasFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'finPrestacaoContas/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'finPrestacaoContas/EditModal');

    var dataTable = $('#finPrestacaoContasTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.finPrestacaoContas.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.finPrestacaoContas.Delete'),
                            confirmMessage: function (data) {
                                return l('finPrestacaoContasDeletionConfirmationMessage', data.record.id);
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
                title: l('finPrestacaoContas:idPrestacao'),
                data: "idPrestacao",
            },
            {
                title: l('finPrestacaoContas:idLancamento'),
                data: "idLancamento",
            },
            {
                title: l('finPrestacaoContas:levantado'),
                data: "levantado",
            },
            {
                title: l('finPrestacaoContas:irpj'),
                data: "irpj",
            },
            {
                title: l('finPrestacaoContas:carta'),
                data: "carta",
            },
            {
                title: l('finPrestacaoContas:honorarios'),
                data: "honorarios",
            },
            {
                title: l('finPrestacaoContas:tarifa'),
                data: "tarifa",
            },
            {
                title: l('finPrestacaoContas:liquidoRecebido'),
                data: "liquidoRecebido",
            },
            {
                title: l('finPrestacaoContas:tsInclusao'),
                data: "tsInclusao",
                dataFormat: 'datetime'
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfinPrestacaoContasButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
