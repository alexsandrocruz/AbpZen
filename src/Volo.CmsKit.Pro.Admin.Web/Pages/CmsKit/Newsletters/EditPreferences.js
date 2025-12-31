abp.modals = abp.modals || {};
abp.modals.EditPreferencesModel = function () {
    function initModal(){
        $('#selectAll').click(function (event) {
            if (this.checked) {
                $('.newsletter-preference-check').each(function () {
                    $(this).prop('checked', true);
                });
            } else {
                $('.newsletter-preference-check').each(function () {
                    $(this).prop('checked', false);
                });
            }
        });

        $(".newsletter-preference-check").on("change", setSelectAll);
        
        function setSelectAll() {
            if ($('.newsletter-preference-check:checked').length === $('.newsletter-preference-check').length) {
                $('#selectAll').prop('checked', true);
            } else {
                $('#selectAll').prop('checked', false);
            }
        }
        
        setSelectAll();
    }
    
    return {
        initModal: initModal
    };
}