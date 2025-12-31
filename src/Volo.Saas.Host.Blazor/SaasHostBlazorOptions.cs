namespace Volo.Saas.Host.Blazor;

public class SaasHostBlazorOptions
{
    public bool EnableTenantImpersonation { get; set; }

    public SaasHostBlazorOptions()
    {
        EnableTenantImpersonation = false;
    }
}
