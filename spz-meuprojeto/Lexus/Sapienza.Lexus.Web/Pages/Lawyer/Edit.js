$(function () {
    var l = abp.localization.getResource('Sapienza.Lexus');
    // ---------- LawyerSpecializations Child Grid ----------
    var _lawyerSpecializationsTable = $('#LawyerSpecializationsTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: false,
            searching: false,
            paging: false,
            info: false,
            scrollX: true,
            data: typeof lawyerSpecializationsInitialData !== 'undefined' ? lawyerSpecializationsInitialData : [],
            columnDefs: [
                {
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

    $('#AddLawyerSpecializationBtn').click(function () {var name = prompt("Select LawyerSpecializations (Placeholder)");
        if (name) {
            _lawyerSpecializationsTable.row.add({
                id: abp.utils.createGuid(),
                : name,
            }).draw();
        }});

    // Form Submit Sync
    $('#EditLawyerForm').submit(function (e) {
        var $form = $(this);
        var data = _lawyerSpecializationsTable.data().toArray();
        data.forEach(function (item, index) {$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].LawyerId" value="' + (item.lawyerId || '') + '" />');$form.append('<input type="hidden" name="ViewModel.LawyerSpecializations[' + index + '].SpecializationId" value="' + (item.specializationId || '') + '" />');
        });
        return true;
    });
});
