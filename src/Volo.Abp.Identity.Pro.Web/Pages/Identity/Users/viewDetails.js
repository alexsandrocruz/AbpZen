var abp = abp || {};
$(function () {
    abp.modals.viewDetailsUser = function () {
        var initModal = function (publicApi, args) {
            
            var l = abp.localization.getResource('AbpIdentity');
            

            $("#JsTreeCheckable").bind("loaded.jstree", function (event, data) {
                $(this).find('.jstree-anchor').each(function (index, element) {
                    var id = $(element).attr('checkbox-id');
                    if ($('#' + id).val() === 'True') {
                        data.instance.check_node(element);
                    }
                    data.instance.disable_checkbox(element);
                })
            });

            $('#JsTreeCheckable').jstree({
                "checkbox": {
                    "keep_selected_style": false,
                    "three_state": false,
                    "tie_selection": false,
                    "whole_node": false
                },
                "plugins": ["checkbox"]
            });

        };
        return {
            initModal: initModal
        };
    };
});
