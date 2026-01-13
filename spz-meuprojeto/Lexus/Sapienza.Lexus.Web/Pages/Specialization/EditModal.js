(function ($) {
    var l = abp.localization.getResource('Sapienza.Lexus');

    abp.modals.EditSpecialization = function () {
        var init = function (modalManager) {
            var $modal = modalManager.getModal();
            var $form = $modal.find('form');
            // ---------- LawyerSpecializations Child Grid ----------
            var _lawyerSpecializationsTable = $('#LawyerSpecializationsTable').DataTable(
                abp.libs.datatables.normalizeConfiguration({
                    processing: true,
                    serverSide: false,
                    searching: false,
                    paging: false,
                    info: false,
                    scrollX: true,
                    columnDefs: [
                        {
                            title: l('Actions'),
                            rowAction: {
                                items: [
                                    {
                                        text: l('Delete'),
                                        action: function (data) {
                                            _lawyerSpecializationsTable.row(data.record.row).remove().draw();
                                        }
                                    }
                                ]
                            }
                        },{
                            title: l('LawyerSpecialization:LawyerId'),
                            data: "lawyerDisplayName",
                            defaultContent: ""
                        },{
                            title: l('LawyerSpecialization:SpecializationId'),
                            data: "specializationDisplayName",
                            defaultContent: ""
                        },
                    ]
                })
            );

            // Load existing data? 
            // In Edit modal, child items should probably be part of the ViewModel and serialized to JS.
            // TODO: Pre-populate _lawyerSpecializationsTable with initial data

            $modal.find('#AddLawyerSpecializationBtn').click(function () {var name = prompt("Select LawyerSpecializations (Placeholder)");
                if (name) {
                    _lawyerSpecializationsTable.row.add({
                        id: abp.utils.createGuid(),
                        : name,
                    }).draw();
                }});

            $form.on('submit', function (e) {
                var data = _lawyerSpecializationsTable.data().toArray();
                data.forEach(function (item, index) {$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].LawyerId" value="' + (item.lawyerId || '') + '" />');$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].SpecializationId" value="' + (item.specializationId || '') + '" />');
                });
            });

            $modal.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            });
        };

        return {
            init: init
        };
    };
})(jQuery);
