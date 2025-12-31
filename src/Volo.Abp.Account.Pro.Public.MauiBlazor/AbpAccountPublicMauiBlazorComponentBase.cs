using Volo.Abp.Account.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.Account.Pro.Public.MauiBlazor;

public abstract class AbpAccountPublicMauiBlazorComponentBase : AbpComponentBase
{
    protected AbpAccountPublicMauiBlazorComponentBase()
    {
        LocalizationResource = typeof(AccountResource);
    }
}
