using Volo.Abp.Application.Services;
using Volo.Abp.Gdpr.Localization;

namespace Volo.Abp.Gdpr;

public abstract class GdprAppServiceBase : ApplicationService
{
    protected GdprAppServiceBase()
    {
        ObjectMapperContext = typeof(AbpGdprApplicationModule);
        LocalizationResource = typeof(AbpGdprResource);
    }
}