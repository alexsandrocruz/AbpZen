using Volo.Abp.AspNetCore.Components;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Admin.Blazor;

public class CmsKitProComponentBase : AbpComponentBase
{
    public CmsKitProComponentBase()
    {
        LocalizationResource = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitProAdminBlazorModule);
    }
}