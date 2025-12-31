using Volo.Abp.TextTemplateManagement.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Abp.TextTemplateManagement;

public abstract class TextTemplateManagementController : AbpControllerBase
{
    protected TextTemplateManagementController()
    {
        LocalizationResource = typeof(TextTemplateManagementResource);
    }
}
