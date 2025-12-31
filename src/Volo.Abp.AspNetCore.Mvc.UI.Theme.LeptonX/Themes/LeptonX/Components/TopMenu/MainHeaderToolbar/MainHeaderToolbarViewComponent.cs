using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.LeptonX.Themes.LeptonX.Components.TopMenu.MainHeaderToolbar;
public class MainHeaderToolbarViewComponent : LeptonXViewComponentBase
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Themes/LeptonX/Components/TopMenu/MainHeaderToolbar/Default.cshtml");
    }
}
