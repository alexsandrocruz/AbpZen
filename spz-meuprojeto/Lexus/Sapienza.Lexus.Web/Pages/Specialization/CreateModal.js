(function ($) {
    var l = abp.localization.getResource('Sapienza.Lexus');

    abp.modals.CreateSpecialization = function () {
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

            $modal.find('#AddLawyerSpecializationBtn').click(function () {// Many-to-Many: Open Lookup Modal for Target Entity
                // For now, placeholder prompt
                var name = prompt("Select LawyerSpecializations (Placeholder)");
                if (name) {
                    _lawyerSpecializationsTable.row.add({
                        id: abp.utils.createGuid(),
                        : name,
                        // Add other junction fields with defaults
                    }).draw();
                }});

            // Sync with form submission
            $form.on('submit', function (e) {
                var data = _lawyerSpecializationsTable.data().toArray();
                data.forEach(function (item, index) {$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].LawyerId" value="' + (item.lawyerId || '') + '" />');$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].SpecializationId" value="' + (item.specializationId || '') + '" />');
                });
            });

            // Adjust columns on tab change
            $modal.find('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
            });
        };

        return {
            init: init
        };
    };
})(jQuery);
