$(function () {
    var tenantSwitchModal = new abp.ModalManager(abp.appPath + "Abp/MultiTenancy/TenantSwitchModal");

    $('#tenant-switch-button').on('click', function (e) {
        e.preventDefault();
        tenantSwitchModal.open();
    });

    tenantSwitchModal.onResult(function () {
        location.reload();
    });
});