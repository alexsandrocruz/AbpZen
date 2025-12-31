using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Chat.Localization;
using Volo.Chat.Settings;

namespace Volo.Chat.Blazor.Pages.Chat.ChatSettingGroup;

public partial class ChatSettingManagementComponent
{
    [Inject]
    protected ISettingsAppService SettingsAppService { get; set; }

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    [Inject]
    protected IStringLocalizer<ChatResource> L { get; set; }

    protected ChatSettingsViewModel Settings;

    protected async override Task OnInitializedAsync()
    {
        Settings = new ChatSettingsViewModel()
        {
            ChatSettings = await SettingsAppService.GetAsync()
        };
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            await SettingsAppService.UpdateAsync(Settings.ChatSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}

public class ChatSettingsViewModel
{
    public ChatSettingsDto ChatSettings { get; set; }
}