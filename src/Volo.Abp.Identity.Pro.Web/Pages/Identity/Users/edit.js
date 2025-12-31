var abp = abp || {};
$(function () {
    abp.modals.editUser = function () {
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();

            var l = abp.localization.getResource('AbpIdentity');
            var rolesCount = $('#RolesCount').val();
            var ouCount  = 0;

            $('#Roles :checkbox').change(function () {
                if (this.checked) {
                    rolesCount++;
                    $('#Roles-tab').text(l('Roles{0}', rolesCount));
                } else {
                    rolesCount--;
                    $('#Roles-tab').text(l('Roles{0}', rolesCount));
                }
            });

            $("#JsTreeCheckable").bind("loaded.jstree", function (event, data) {
                $(this).find('.jstree-anchor').each(function (index, element) {
                    var id = $(element).attr('checkbox-id');
                    if ($('#' + id).val() === 'True') {
                        data.instance.check_node(element);
                    }
                })
            });

            $("#JsTreeCheckable").on("check_node.jstree uncheck_node.jstree", function (e, data) {
                $('#' + data.node.a_attr["checkbox-id"]).prop("checked", (data.node.state.checked));
                $('#' + data.node.a_attr["checkbox-id"]).val(data.node.state.checked ? "True" : "False");
                ouCount = data.node.state.checked ? ouCount+1: ouCount-1;
                $('#OrganizationUnits-tab').text(l('OrganizationUnits{0}', ouCount));

                if(data.node.a_attr.data_roles){
                    var roles = data.node.a_attr.data_roles.split(',');
                    if(roles.length > 0){
                        for(var i = 0; i < roles.length; i++){
                            var role = roles[i];
                            var roleElement = $('#Roles').find('input[data-role="' + role + '"]');
                            
                            if(data.node.state.checked){
                                roleElement.next().text(role+ " (OU)");
                                roleElement.attr("disabled","disabled");
                            }

                            if(data.node.state.checked && roleElement.is(':checked'))
                                continue;

                            if(data.node.state.checked){
                                roleElement.prop("checked", true)
                            }else{
                                roleElement.next().text(role);
                                roleElement.enable();
                                roleElement.prop("checked", false)
                            }
                            roleElement.trigger('change');
                        }
                    }
                }
            });

            $('#JsTreeCheckable').jstree({
                "checkbox": {
                    "keep_selected_style": false,
                    "three_state": false,
                    "tie_selection": false
                },
                "plugins": ["checkbox"]
            });

        };
        return {
            initModal: initModal
        };
    };
});
