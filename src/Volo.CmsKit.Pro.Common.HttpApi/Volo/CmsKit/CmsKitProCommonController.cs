using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

public abstract class CmsKitProCommonController : AbpControllerBase
{
    protected CmsKitProCommonController()
    {
        LocalizationResource = typeof(CmsKitResource);
    }
}
