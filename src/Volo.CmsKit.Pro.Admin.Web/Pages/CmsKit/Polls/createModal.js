var abp = abp || {};
$(function () {
    var l = abp.localization.getResource("CmsKit");
    abp.modals.newPollButton = function () {
        var initModal = function () {
            var pollIndex = $('#OptionStartIndex').val();
            var singleDatePickers = $('.singledatepicker');
            var $question = $("#ViewModel_Question");
            var $name = $("#ViewModel_Name");
            singleDatePickers.attr('autocomplete', 'off');
            moment()._locale.preparse = (string) => string;
            moment()._locale.postformat = (string) => string;
            
            singleDatePickers.each(function () {
                var $this = $(this);
                $this.daterangepicker({
                    "singleDatePicker": true,
                    "showDropdowns": true,
                    "autoUpdateInput": false,
                    "autoApply": true,
                    "opens": "center",
                    "drops": "auto",
                    "timePicker": true,
                }, function (start, end, label) {
                    $this.val(start.format('YYYY-MM-DD HH:mm A'));
                });
                if($this.val()) {
                    var isoDate = moment($this.val());
                    $this.val(isoDate.format('YYYY-MM-DD HH:mm A'));
                    $this.data('daterangepicker').setStartDate(isoDate);
                }
            });

            var entityMap = {
                '&': '&amp;',
                '<': '&lt;',
                '>': '&gt;',
                '"': '&quot;',
                "'": '&#39;',
                '/': '&#x2F;',
                '`': '&#x60;',
                '=': '&#x3D;'
            };

            function escapeHtml(string) {
                return String(string).replace(/[&<>"'`=\/]/g, function (s) {
                    return entityMap[s];
                });
            }

            var getOptionTableRow = function (text) {
                var escapedHtml = escapeHtml(text);
                return "<tr>\r\n<td>\r\n" + escapedHtml + "\r\n </td>" +
                    "<td class=\"text-end\">" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-dark createUpOptionButton\"><i class=\"fa fa-arrow-up\"></i></button>" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-dark createDownOptionButton\"><i class=\"fa fa-arrow-down\"></i></button>" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-danger createDeleteOptionButton\"><i class=\"fa fa-trash\"></i></button>" +
                    "</td > " +
                    "                                        <td hidden >" +
                    "                                       <input type=\"text\" data-text name=\"ViewModel.PollOptions[" + pollIndex + "].Text\" value=\"" + text + "\"/>" +
                    "                                       <input type=\"text\" data-order name=\"ViewModel.PollOptions[" + pollIndex + "].Order\" value=\"" + (Number(pollIndex) + 1) + "\"id=\"" + text + "\">" +
                    "</td></tr>";
            }

            var addTextToTableRow = function () {
                var optionText = $("#NewOption").val();

                if (!optionText) {
                    return;
                }

                $("#NewOption").val("");

                var html = getOptionTableRow(optionText);

                $("#CreateOptionTableBodyId").append(html);

                pollIndex++;
                $("#CreateOptionTableId").show();
            }

            $("#CreateAddNewOptionButton").on('click', function () {
                addTextToTableRow();
            });

            function isDoubleClicked(element) {
                if (element.data("isclicked")) return true;

                element.data("isclicked", true);
                setTimeout(function () {
                    element.removeData("isclicked");
                }, 500);
            }

            $question.on("change keyup paste", function(){
                var question = $question.val().toLocaleLowerCase();
                var slugified = slugify(question, {
                    lower: true
                });
                
                if(slugified != $name.val()) {
                    $name.val(slugified);
                }
            })

            $("#NewOption").keypress(function (event) {
                let keyCode = (event.keyCode ? event.keyCode : event.which);
                if (keyCode == "13") {
                    addTextToTableRow();
                    return false;
                }
            });

            $(document).on('click', '.createDeleteOptionButton', function () {
                var $row = $(this).closest('tr');
                $row.remove();
            });

            $(document).on('click', '.createUpOptionButton', function () {

                if (!$(this).closest("tr").is(":first-child")) {

                    if (isDoubleClicked($(this))) return;

                    var row = $(this).parents("tr:first");
                    var $thirdHiddenInput2 = row.find('td').eq(2);
                    var hiddenInput = $thirdHiddenInput2.find('input')[1];

                    var name = "input[id='" + hiddenInput.id + "']";
                    $(name).attr('value', --hiddenInput.value);

                    var previousRow = row.prev();
                    var $previousHiddenInput = previousRow.find('td').eq(2);
                    var previousHiddenInput = $previousHiddenInput.find('input')[1];

                    var previosuName = "input[id='" + previousHiddenInput.id + "']";
                    $(previosuName).attr('value', ++previousHiddenInput.value);
                    row.insertBefore(row.prev());
                }

            });

            $(document).on('click', '.createDownOptionButton', function () {


                if (!$(this).closest("tr").is(":last-child")) {

                    if (isDoubleClicked($(this))) return;

                    var row = $(this).parents("tr:first");
                    var $thirdHiddenInput2 = row.find('td').eq(2);
                    var hiddenInput = $thirdHiddenInput2.find('input')[1];

                    var name = "input[id='" + hiddenInput.id + "']";
                    $(name).attr('value', ++hiddenInput.value);

                    var nextRow = row.next();
                    var $nextHiddenInput = nextRow.find('td').eq(2);
                    var nextHiddenInput = $nextHiddenInput.find('input')[1];

                    var previosuName = "input[id='" + nextHiddenInput.id + "']";
                    $(previosuName).attr('value', --nextHiddenInput.value);

                    row.insertAfter(row.next());
                }
            });
        };

        return {
            initModal: initModal
        };
    };

    function reIndexOptions(){
        var $optionsTBody = $('#CreateOptionTableBodyId');
        $optionsTBody.find("tr").each(function(index){
            var $hiddenTd = $(this).find("td[hidden]");
            $hiddenTd.find("[data-id]").attr("name","ViewModel.PollOptions["+index+"].Id")
            $hiddenTd.find("[data-text]").attr("name","ViewModel.PollOptions["+index+"].Text");
            $hiddenTd.find("[data-order]").attr("name","ViewModel.PollOptions["+index+"].Order");
        });
    }

    $(document).on('change', '#ViewModel_EndDate', function () {
        $('#ViewModel_ShowHoursLeft').attr('checked', 'checked')
    });

    $(document).on('click', '#SavePoll', function () {
        reIndexOptions();
        var rowCount = $('#CreateOptionTableBodyId tr').length;
        if (rowCount < 2) {
            abp.message.warn(l('Poll:YouShouldCreateAtLeastTwoOptions'));
            return false;
        }
    });
});