namespace Volo.Abp.Identity.Pro.Blazor;

public class AbpIdentityProBlazorOptions
{
    public bool EnableUserImpersonation { get; set; }

    public AbpIdentityProBlazorOptions()
    {
        EnableUserImpersonation = false;
    }
}
