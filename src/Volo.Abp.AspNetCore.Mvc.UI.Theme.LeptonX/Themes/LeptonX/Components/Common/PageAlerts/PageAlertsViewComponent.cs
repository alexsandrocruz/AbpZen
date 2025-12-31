using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Alerts;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.Common.PageAlerts;
public class PageAlertsViewComponent : LeptonXViewComponentBase
{
    protected IAlertManager AlertManager { get; }

    public PageAlertsViewComponent(IAlertManager alertManager)
    {
        AlertManager = alertManager;
    }

    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/LeptonX/Components/Common/PageAlerts/Default.cshtml", AlertManager.Alerts);
    }
}
