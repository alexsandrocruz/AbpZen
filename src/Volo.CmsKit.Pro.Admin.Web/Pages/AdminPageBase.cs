using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Layout;
using Volo.CmsKit.Localization;

namespace Volo.CmsKit.Pro.Admin.Web.Pages;

public class AdminPageBase : Microsoft.AspNetCore.Mvc.RazorPages.Page
{
    [RazorInject]
    public IStringLocalizer<CmsKitResource> L { get; set; }
    [RazorInject]
    public IPageLayout PageLayout { get; set; }

    public override Task ExecuteAsync()
    {
        return Task.CompletedTask; // Will be overriden by razor pages. (.cshtml)
    }
}
