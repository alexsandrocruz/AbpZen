using System;
using Volo.Abp.Application.Services;
using Volo.Abp.OpenIddict.Localization;

namespace Volo.Abp.OpenIddict;

public abstract class AbpOpenIddictProAppService : ApplicationService
{
    protected AbpOpenIddictProAppService()
    {
        LocalizationResource = typeof(AbpOpenIddictResource);
        ObjectMapperContext = typeof(AbpOpenIddictProApplicationModule);
    }
    
    protected virtual Guid ConvertIdentifierFromString(string identifier)
    {
        return string.IsNullOrEmpty(identifier) ? default : Guid.Parse(identifier);
    }

    protected virtual string ConvertIdentifierToString(Guid identifier)
    {
        return identifier.ToString("D");
    }
}
