using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.LanguageManagement.Blazor.Pages.LanguageManagement.Components;

public partial class IsEnabledColumnComponent : AbpComponentBase
{
    [Parameter]
    public object Data { get; set; }
}
