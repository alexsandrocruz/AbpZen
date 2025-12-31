using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Features;

namespace Volo.Chat.Web.Pages.Chat;

[RequiresFeature(ChatFeatures.Enable)]
public class IndexModel : AbpPageModel
{
    public void OnGet()
    {
    }
}
