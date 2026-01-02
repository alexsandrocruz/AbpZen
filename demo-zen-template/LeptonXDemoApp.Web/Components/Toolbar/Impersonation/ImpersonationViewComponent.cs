using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace LeptonXDemoApp.Web.Components.Toolbar.Impersonation
{
    public class ImpersonationViewComponent : AbpViewComponent
    {
        public virtual IViewComponentResult Invoke()
        {
            return View("~/Components/Toolbar/Impersonation/Default.cshtml");
        }
    }
}
