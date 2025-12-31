using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.Common.MainHeaderBranding;
public class MainHeaderBrandingViewComponent : LeptonXViewComponentBase
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/LeptonX/Components/Common/MainHeaderBranding/Default.cshtml");
    }
}
