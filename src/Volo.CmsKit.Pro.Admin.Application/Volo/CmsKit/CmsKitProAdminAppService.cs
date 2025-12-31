using Volo.Abp.Application.Services;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit;

public abstract class CmsKitProAdminAppService : ApplicationService
{
    protected CmsKitProAdminAppService()
    {
        LocalizationResource = typeof(CmsKitResource);
        ObjectMapperContext = typeof(CmsKitProAdminApplicationModule);
    }
}
