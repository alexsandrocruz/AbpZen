(function ($) {

    var l = abp.localization.getResource('Saas');

    let _impersonateTenantModal = new abp.ModalManager({
        viewUrl: abp.appPath + 'Saas/Host/Tenants/ImpersonateTenantModal',
        modalClass: 'ImpersonateTenantManagement'
    });

    abp.ui.extensions.entityActions.get("saas.tenant").addContributor(
        function(actionList) {
            return actionList.addBefore(
                {
                    text: l('LoginWithThisTenant'),
                    visible: function (data) {
                        return (data.activationState == 0 || (data.activationState == 1 && new Date(data.activationEndDate) > new Date())) && 
                            abp.auth.isGranted('Saas.Tenants.Impersonation') &&
                            abp.currentUser.impersonatorUserId == null;
                    },
                    action: function (data) {
                        _impersonateTenantModal.open({
                            tenantId: data.record.id,
                            tenantName : data.record.name
                        });
                    }
                },
                l('Delete'),
                (value, searchedValue) => value.text === searchedValue
            );
        }
    );

})(jQuery);
