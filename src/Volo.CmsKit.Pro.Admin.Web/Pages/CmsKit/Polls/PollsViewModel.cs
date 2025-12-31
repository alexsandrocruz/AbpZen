using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Volo.CmsKit.Pro.Admin.Web.Pages.CmsKit.Polls;

public class PollsViewModel
{
    public List<SelectListItem> Widgets { get; set; } = new();

    [SelectItems(nameof(Widgets))]
    public string Widget { get; set; }
}
