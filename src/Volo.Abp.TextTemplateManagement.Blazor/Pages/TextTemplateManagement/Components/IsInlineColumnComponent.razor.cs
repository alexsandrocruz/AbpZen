using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Abp.TextTemplateManagement.Blazor.Pages.TextTemplateManagement.Components;

public partial class IsInlineColumnComponent : AbpComponentBase
{
    [Parameter]
    public object Data { get; set; }
}
