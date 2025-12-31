var abp = abp || {};
$(function () {
    abp.modals.createUser = function () {
        
        var togglePasswordVisibility = function () {
            $("#PasswordVisibilityButton").click(function (e) {
                var button = $(this);
                var passwordInput = button.parent().find("input");
                if(!passwordInput) {
                    return;
                }

                if(passwordInput.attr("type") === "password") {
                    passwordInput.attr("type", "text");
                }
                else {
                    passwordInput.attr("type", "password");
                }

                var icon = button.find("i");
                if(icon) {
                    icon.toggleClass("fa-eye-slash").toggleClass("fa-eye");
                }
            });
        }
        
        var initModal = function (publicApi, args) {
            var $form = publicApi.getForm();
            var l = abp.localization.getResource('AbpIdentity');
            var rolesCount = $('#RolesCount').val();
            var ouCount  = 0;
            
            $("#JsTreeCheckable").on("check_node.jstree uncheck_node.jstree", function (e, data) {
                $('#' + data.node.a_attr["checkbox-id"]).prop("checked", (data.node.state.checked));
                $('#' + data.node.a_attr["checkbox-id"]).val(data.node.state.checked ? "True" : "False");
            });

            $('#JsTreeCheckable').jstree({
                "checkbox": {
                    "keep_selected_style": false,
                    "three_state": false,
                    "tie_selection": false
                },
                "plugins": ["checkbox"]
            });

            $('#Roles :checkbox').change(function () {
                if (this.checked) {
                    rolesCount++;
                    $('#Roles-tab').text(l('Roles{0}', rolesCount));
                } else {
                    rolesCount--;
                    $('#Roles-tab').text(l('Roles{0}', rolesCount));
                }
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

            togglePasswordVisibility();
        };
        return {
            initModal: initModal
        };
    };
});
