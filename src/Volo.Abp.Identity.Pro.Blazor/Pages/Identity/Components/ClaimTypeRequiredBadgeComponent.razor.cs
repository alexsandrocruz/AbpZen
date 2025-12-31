using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.Components;

public partial class ClaimTypeRequiredBadgeComponent : ComponentBase
{
    [Parameter]
    public object Data { get; set; }
}
