using Microsoft.AspNetCore.Components;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

public partial class RoleNameComponent : ComponentBase
{
    [Parameter]
    public object Data { get; set; }
}
