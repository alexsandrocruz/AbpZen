using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Features;
using Volo.Abp.SettingManagement.Blazor;
using Volo.Chat.Authorization;
using Volo.Chat.Blazor.Pages.Chat.ChatSettingGroup;
using Volo.Chat.Localization;

namespace Volo.Chat.Blazor.Settings;

public class ChatSettingManagementComponentContributor : ISettingComponentContributor
{
    public async Task ConfigureAsync(SettingComponentCreationContext context)
    {
        if (!await CheckFeatureAsync(context))
        {
            return;
        }

        if (!await CheckPermissionsAsync(context))
        {
            return;
        }
        
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<ChatResource>>();
        context.Groups.Add(
            new SettingComponentGroup(
                "Volo.Abp.Chat",
                l["Menu:Chat"],
                typeof(ChatSettingManagementComponent)
            )
        );
    }

    public virtual async Task<bool> CheckPermissionsAsync(SettingComponentCreationContext context)
    {
        var authorizationService = context.ServiceProvider.GetRequiredService<IAuthorizationService>();

        return await authorizationService.IsGrantedAsync(ChatPermissions.SettingManagement);
    }
    
    public virtual async Task<bool> CheckFeatureAsync(SettingComponentCreationContext context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();
        return await featureChecker.IsEnabledAsync(ChatFeatures.Enable);
    }
}
