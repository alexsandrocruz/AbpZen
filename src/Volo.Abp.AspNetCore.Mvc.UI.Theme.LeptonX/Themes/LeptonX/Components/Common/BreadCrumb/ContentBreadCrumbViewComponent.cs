using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.Common.BreadCrumb;

public class ContentBreadCrumbViewComponent : LeptonXViewComponentBase
{
    protected IPageLayout PageLayout { get; }

    public ContentBreadCrumbViewComponent(IPageLayout pageLayout)
    {
        PageLayout = pageLayout;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/LeptonX/Components/Common/BreadCrumb/Default.cshtml", PageLayout.Content);
    }
}
