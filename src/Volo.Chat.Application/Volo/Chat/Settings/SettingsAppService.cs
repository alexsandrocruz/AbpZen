using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement;
using Volo.Abp.Settings;
using Volo.Chat.Authorization;
using Volo.Chat.Messages;

namespace Volo.Chat.Settings;

[Authorize(ChatPermissions.SettingManagement)]
[RequiresFeature(ChatFeatures.Enable)]
public class SettingsAppService : ChatAppService, ISettingsAppService
{
    protected ISettingManager SettingManager { get; }

    public SettingsAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    public virtual async Task SetSendOnEnterSettingAsync(SendOnEnterSettingDto input)
    {
        await SettingManager.SetForCurrentUserAsync(ChatSettingNames.Messaging.SendMessageOnEnter, input.SendOnEnter.ToString());
    }

    public async Task<ChatSettingsDto> GetAsync()
    {
        return new ChatSettingsDto
        {
            DeletingMessages = Enum.Parse<ChatDeletingMessages>(await SettingProvider.GetOrNullAsync(ChatSettingNames.Messaging.DeletingMessages)),
            MessageDeletionPeriod = await SettingProvider.GetAsync<int>(ChatSettingNames.Messaging.MessageDeletionPeriod),
            DeletingConversations = Enum.Parse<ChatDeletingConversations>(await SettingProvider.GetOrNullAsync(ChatSettingNames.Messaging.DeletingConversations))
        };
    }

    public async Task UpdateAsync(ChatSettingsDto input)
    {
        if (input != null)
        {
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, ChatSettingNames.Messaging.DeletingMessages,input.DeletingMessages.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, ChatSettingNames.Messaging.MessageDeletionPeriod, input.MessageDeletionPeriod.ToString());
            await SettingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, ChatSettingNames.Messaging.DeletingConversations, input.DeletingConversations.ToString());
        }
    }
}
