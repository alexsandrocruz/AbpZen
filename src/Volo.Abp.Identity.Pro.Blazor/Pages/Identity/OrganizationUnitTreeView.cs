using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity;

public class OrganizationUnitTreeView : OrganizationUnitWithDetailsDto
{
    public bool HasChildren => Children?.Any() ?? false;

    public List<OrganizationUnitTreeView> Children { get; set; }

    public bool Collapsed { get; set; } = true;

    public string Icon => Collapsed ? "fa-angle-right" : "fa-angle-down";
}
