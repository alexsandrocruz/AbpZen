using Volo.Forms.Localization;
using Volo.Abp.Application.Services;

namespace Volo.Forms;

public abstract class FormsAppServiceBase : ApplicationService
{
    protected FormsAppServiceBase()
    {
        LocalizationResource = typeof(FormsResource);
        ObjectMapperContext = typeof(FormsApplicationModule);
    }
}
