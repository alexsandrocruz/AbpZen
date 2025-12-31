var abp = abp || {};
$(function () {
    
    var l = abp.localization.getResource("CmsKit");

    abp.modals.editPoll = function () {

        var initModal = function () {


            var singleDatePickers = $('.singledatepicker');
            singleDatePickers.attr('autocomplete', 'off')
            var $question = $("#ViewModel_Question");
            var $name = $("#ViewModel_Name");
            moment()._locale.preparse = (string) => string;
            moment()._locale.postformat = (string) => string;
            
            var dateFormat = 'YYYY-MM-DD HH:mm A';

            singleDatePickers.each(function () {
                var $this = $(this);
                $this.daterangepicker({
                    "singleDatePicker": true,
                    "showDropdowns": true,
                    "autoUpdateInput": false,
                    "autoApply": true,
                    "opens": "center",
                    "drops": "auto",
                    "timePicker": true
                }, function (start, end, label) {
                    $this.val(start.format(dateFormat));
                });
                if($this.val()) {
                    var isoDate = moment($this.val());
                    $this.val(isoDate.format(dateFormat));
                    $this.data('daterangepicker').setStartDate(isoDate);
                }
            });

            var editTextModal = new abp.ModalManager({ viewUrl: abp.appPath + "CmsKit/Polls/EditTextModal", modalClass: "pollEditTextModal" });

            var pollIndex = $('#OptionStartIndex').val();

            var guid = function createUUID() {
                // http://www.ietf.org/rfc/rfc4122.txt
                var s = [];
                var hexDigits = "0123456789abcdef";
                for (var i = 0; i < 36; i++) {
                    s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
                }
                s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
                s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
                s[8] = s[13] = s[18] = s[23] = "-";

                return s.join("");
            }

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

            var getOptionTableRow = function (id, text) {
                var escapedHtml = escapeHtml(text);
                return "<tr><td> " + escapedHtml + " </td>" +
                    "<td class=\"text-end\">" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-dark upOptionButton\"><i class=\"fa fa-arrow-up\"></i></button>" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-dark downOptionButton\"><i class=\"fa fa-arrow-down\"></i></button>" +
                    "                                       <button type=\"button\" class=\"btn btn-outline-danger deleteOptionButton\"><i class=\"fa fa-trash\"></i></button>" +
                    "</td > " +
                    "<td hidden >" +
                    "                                       <input type=\"text\" data-id name=\"ViewModel.PollOptions[" + pollIndex + "].Id\" value=\"" + id + "\" id=\"" + id + "\">" +
                    "                                       <input type=\"text\" data-text name=\"ViewModel.PollOptions[" + pollIndex + "].Text\" value=\"" + text + "\"id=\"" + text + "\">" +
                    "                                       <input type=\"text\" data-order name=\"ViewModel.PollOptions[" + pollIndex + "].Order\" value=\"" + (Number(pollIndex) + 1) + "\" id=\"" + id + "\">" +
                    "</td></tr>";
            }

            var addTextToTableRow = function () {
                var text = $("#NewOption").val();

                if (!text) {
                    return;
                }

                var id = guid();

                $("#NewOption").val("");

                var html = getOptionTableRow(id, text);
                $("#OptionTableBodyId").append(html);

                pollIndex++;
                $("#OptionTableId").show();
            }

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

            $(".editOptionButton").click(function () {
                var hasHighlight = $('#OptionTableBodyId').children().hasClass('highlight');
                if (hasHighlight === true) {
                    $('#OptionTableBodyId').children().removeClass('highlight');
                }

                var $id = $(this).closest("tr")
                $id.addClass('highlight');
                editTextModal.open();
            });

            $("#AddNewOptionButton").click(function () {
                addTextToTableRow();
            });

            $("#NewOption").keypress(function (event) {
                let keyCode = (event.keyCode ? event.keyCode : event.which);
                if (keyCode == "13") {
                    addTextToTableRow();
                    return false;
                }
            });

            $(document).on('click', '.deleteOptionButton', function () {
                var $row = $(this).closest("tr")

                $row.remove();
            });

            $(document).on('click', '.upOptionButton', function () {

                if (!$(this).closest("tr").is(":first-child")) {

                    if (isDoubleClicked($(this))) return;

                    var row = $(this).parents("tr:first");
                    var $thirdHiddenInput2 = row.find('td').eq(2);
                    var hiddenInput = $thirdHiddenInput2.find('input')[2];

                    var name = "input[name='" + hiddenInput.name + "']";
                    $(name).attr('value', --hiddenInput.value);

                    var previousRow = row.prev();
                    var $previousHiddenInput = previousRow.find('td').eq(2);
                    var previousHiddenInput = $previousHiddenInput.find('input')[2];

                    var previousName = "input[name='" + previousHiddenInput.name + "']";
                    $(previousName).attr('value', ++previousHiddenInput.value);
                    row.insertBefore(row.prev());
                }
            });

            $(document).on('click', '.downOptionButton', function () {

                if (!$(this).closest("tr").is(":last-child")) {

                    if (isDoubleClicked($(this))) return;

                    var row = $(this).parents("tr:first");
                    var $thirdHiddenInput2 = row.find('td').eq(2);
                    var hiddenInput = $thirdHiddenInput2.find('input')[2];

                    var name = "input[name='" + hiddenInput.name + "']";
                    $(name).attr('value', ++hiddenInput.value);

                    var nextRow = row.next();
                    var $nextHiddenInput = nextRow.find('td').eq(2);
                    var nextHiddenInput = $nextHiddenInput.find('input')[2];

                    var previousName = "input[name='" + nextHiddenInput.name + "']";
                    $(previousName).attr('value', --nextHiddenInput.value);

                    row.insertAfter(row.next());
                }
            });
        };

        return {
            initModal: initModal
        };
    };

    function reIndexOptions(){
        var $optionsTBody = $('#OptionTableBodyId');
        $optionsTBody.find("tr").each(function(index){
            var $hiddenTd = $(this).find("td[hidden]");
            $hiddenTd.find("[data-id]").attr("name","ViewModel.PollOptions["+index+"].Id")
            $hiddenTd.find("[data-text]").attr("name","ViewModel.PollOptions["+index+"].Text");
            $hiddenTd.find("[data-order]").attr("name","ViewModel.PollOptions["+index+"].Order");
        });
    }
    
    $(document).on('click', '#UpdatePoll', function () {
        reIndexOptions();
        var rowCount = $('#OptionTableBodyId tr').length;
        if (rowCount < 2) {
            abp.message.warn(l('Poll:YouShouldCreateAtLeastTwoOptions'));
            return false;
        }
    });
});