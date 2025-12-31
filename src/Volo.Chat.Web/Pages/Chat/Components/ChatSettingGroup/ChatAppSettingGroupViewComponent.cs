using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Chat.Settings;

namespace Volo.Chat.Web.Pages.Chat.Components.ChatSettingGroup;

public class ChatAppSettingGroupViewComponent : AbpViewComponent
{
    public ChatSettingsViewModel SettingsViewModel { get; set; }

    protected ISettingsAppService SettingsAppService { get; }

    public ChatAppSettingGroupViewComponent(ISettingsAppService settingsAppService)
    {
        SettingsAppService = settingsAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        SettingsViewModel = new ChatSettingsViewModel {
            ChatSettings = await SettingsAppService.GetAsync()
        };

        return View("~/Pages/Chat/Components/ChatSettingGroup/Default.cshtml", SettingsViewModel);
    }

    public class ChatSettingsViewModel
    {
        public ChatSettingsDto ChatSettings { get; set; }
    }
}