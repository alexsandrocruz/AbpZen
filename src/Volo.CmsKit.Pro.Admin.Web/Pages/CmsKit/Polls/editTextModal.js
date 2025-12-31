var abp = abp || {};
$(function () {
    abp.modals.pollEditTextModal = function () {
        var initModal = function () {

            $("#poll-change-text-button").click(function () {
                var updatedText = $("#textId").val();

                if (!updatedText) {
                    return;
                }

                event.preventDefault();
                var $selectedRow = $('table > tbody > tr.highlight');
                var $firstTd = $selectedRow.find('td').eq(0);
                $firstTd.text(updatedText);

                var $secondHiddenInput = $selectedRow.find('td').eq(2);
                var hiddenInput = $secondHiddenInput.find('input')[1];

                var name = "input[name='" + hiddenInput.name + "']";
                $(name).attr('value', updatedText);
                $(name).attr('id', updatedText);
                $selectedRow.removeClass('highlight');

                $('#editTextModal').modal('hide');
            });
        };

        return {
            initModal: initModal
        };
    };
});