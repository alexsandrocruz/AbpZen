using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Widgets;
using Volo.CmsKit.Public.Faqs;

namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.Faqs;

[Widget]
[ViewComponent(Name = "CmsFaqOptions")]
public class CmsFaqOptionsViewComponent : AbpViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View("~/Pages/Public/Shared/Components/Faqs/CmsFaqOptions.cshtml", new CmsFaqOptionsViewModel());
    }
}