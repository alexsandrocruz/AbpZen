$(function () {
    var l = abp.localization.getResource('LeptonXDemoApp');
    // ---------- AlunoTurmas Child Grid ----------
    var _alunoTurmasTable = $('#AlunoTurmasTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            serverSide: false,
            searching: false,
            paging: false,
            info: false,
            scrollX: true,
            data: typeof alunoTurmasInitialData !== 'undefined' ? alunoTurmasInitialData : [],
            columnDefs: [
                {
                    rowAction: {
                        items: [
                            {
                                text: l('Delete'),
                                action: function (data) {
                                    _alunoTurmasTable.row(data.record.row).remove().draw();
                                }
                            }
                        ]
                    }
                },{
                    title: l('AlunoTurma:AlunoId'),
                    data: "alunoDisplayName",
                    defaultContent: ""
                },{
                    title: l('AlunoTurma:TurmaId'),
                    data: "turmaDisplayName",
                    defaultContent: ""
                },
            ]
        })
    );

    $('#AddAlunoTurmaBtn').click(function () {var name = prompt("Select AlunoTurmas (Placeholder)");
        if (name) {
            _alunoTurmasTable.row.add({
                id: abp.utils.createGuid(),
                : name,
            }).draw();
        }});

    // Form Submit Sync
    $('#EditTurmaForm').submit(function (e) {
        var $form = $(this);
        var data = _alunoTurmasTable.data().toArray();
        data.forEach(function (item, index) {$form.append('<input type="hidden" name="ViewModel.AlunoTurmas[' + index + '].AlunoId" value="' + (item.alunoId || '') + '" />');$form.append('<input type="hidden" name="ViewModel.AlunoTurmas[' + index + '].TurmaId" value="' + (item.turmaId || '') + '" />');$form.append('<input type="hidden" name="ViewModel.AlunoTurmas[' + index + '].DataMatricula" value="' + (item.dataMatricula || '') + '" />');$form.append('<input type="hidden" name="ViewModel.AlunoTurmas[' + index + '].Situacao" value="' + (item.situacao || '') + '" />');
        });
        return true;
    });
});
