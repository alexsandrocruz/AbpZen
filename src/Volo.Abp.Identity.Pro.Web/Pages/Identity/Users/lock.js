var abp = abp || {};
$(function () {
    abp.modals.lock = function () {
        var initModal = function (publicApi, args) {
            var singleDatePickers = $('.singledatepicker');
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
        };
        return {
            initModal: initModal
        };
    };
});