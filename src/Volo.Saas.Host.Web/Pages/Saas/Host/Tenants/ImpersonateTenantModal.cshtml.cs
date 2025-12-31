using System;
using Volo.Saas.Host.Navigation;

namespace Volo.Saas.Host.Pages.Saas.Host.Tenants;

public class ImpersonateTenantModal : SaasHostPageModel
{
    public Guid TenantId { get; set; }
    
    public string TenantName { get; set; }

    public string DefaultAdminUserName { get; set; } = "admin";

    public string ReturnUrl = "/Saas/Host/Tenants";

    public void OnGet(Guid tenantId, string tenantName)
    {
        TenantId = tenantId;
        TenantName = tenantName;
    }
}