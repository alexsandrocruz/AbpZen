using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.Common.ContentTitle;

public class ContentTitleViewComponent : AbpViewComponent
{
    protected IPageLayout PageLayout { get; }

    public ContentTitleViewComponent(IPageLayout pageLayout)
    {
        PageLayout = pageLayout;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View(
             "~/Themes/LeptonX/Components/Common/ContentTitle/Default.cshtml",
             PageLayout.Content);
    }
}
