using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.Chat.Web.Pages.Chat.Components.MessagesToolbarItem;

public class MessagesToolbarItemViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("/Pages/Chat/Components/MessagesToolbarItem/Default.cshtml");
    }
}
