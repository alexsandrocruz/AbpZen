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

    $("#fabConfigFilter :input").on('input', debounce(function () {
        dataTable.ajax.reload();
    }, 300));

    var getFilter = function () {
        var input = {};
        $("#fabConfigFilter")
            .serializeArray()
            .forEach(function (data) {
                if (data.value != '') {
                    input[abp.utils.toCamelCase(data.name.replace(/fabConfigFilter./g, ''))] = data.value;
                }
            })
        return input;
    };

    var l = abp.localization.getResource('Sapienza.Lexus');
    // Standard: Modal CRUD
    var createModal = new abp.ModalManager(abp.appPath + 'fabConfig/CreateModal');
    var editModal = new abp.ModalManager(abp.appPath + 'fabConfig/EditModal');

    var dataTable = $('#fabConfigTable').DataTable(abp.libs.datatables.normalizeConfiguration({
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
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabConfig.Update'),
                            action: function (data) {
                                editModal.open({ id: data.record.id });
                            }
                        },
                        {
                            text: l('Delete'),
                            visible: abp.auth.isGranted('Sapienza.Lexus.fabConfig.Delete'),
                            confirmMessage: function (data) {
                                return l('fabConfigDeletionConfirmationMessage', data.record.id);
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
                title: l('fabConfig:idConfig'),
                data: "idConfig",
            },
            {
                title: l('fabConfig:imagemLogin'),
                data: "imagemLogin",
            },
            {
                title: l('fabConfig:imagemLoginCentral'),
                data: "imagemLoginCentral",
            },
            {
                title: l('fabConfig:imagemLoginTickets'),
                data: "imagemLoginTickets",
            },
            {
                title: l('fabConfig:tsAlteracao'),
                data: "tsAlteracao",
                dataFormat: 'datetime'
            },
            {
                title: l('fabConfig:precoCombustivel'),
                data: "precoCombustivel",
            },
            {
                title: l('fabConfig:dataBloqueioFinanceiro'),
                data: "dataBloqueioFinanceiro",
            },
        ]
    }));
    createModal.onResult(function () {
        dataTable.ajax.reload();
    });

    editModal.onResult(function () {
        dataTable.ajax.reload(null, false);
    });

    $('#NewfabConfigButton').click(function (e) {
        e.preventDefault();
        createModal.open();
    });
});
