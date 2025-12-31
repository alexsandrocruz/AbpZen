using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.LanguageManagement.Localization;

namespace Volo.Abp.LanguageManagement.Blazor.Pages.LanguageManagement.Components;

public partial class LanguageNameColumnComponent : AbpComponentBase
{
    [Parameter]
    public object Data { get; set; }

    public LanguageNameColumnComponent()
    {
        LocalizationResource = typeof(LanguageManagementResource);
    }
}
