using Volo.Abp.AspNetCore.Mvc;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

public abstract class CmsKitProAdminController : AbpControllerBase
{
    protected CmsKitProAdminController()
    {
        LocalizationResource = typeof(CmsKitResource);
    }
}
