namespace Volo.Saas.Host;

public class AbpSaasHostWebOptions
{
    public bool EnableTenantImpersonation { get; set; }

    public AbpSaasHostWebOptions()
    {
        EnableTenantImpersonation = false;
    }
}
