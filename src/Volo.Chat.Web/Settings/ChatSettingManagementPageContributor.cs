using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.SettingManagement.Web.Pages.SettingManagement;
using Volo.Chat.Authorization;
using Volo.Chat.Localization;
using Volo.Chat.Web.Pages.Chat.Components.ChatSettingGroup;

namespace Volo.Chat.Web.Settings;

public class ChatSettingManagementPageContributor : SettingPageContributorBase
{
    public ChatSettingManagementPageContributor()
    {
        RequiredPermissions(ChatPermissions.SettingManagement);
        RequiredFeatures(ChatFeatures.Enable);
    }

    public async override Task ConfigureAsync(SettingPageCreationContext context)
    {
        var l = context.ServiceProvider.GetRequiredService<IStringLocalizer<ChatResource>>();
        context.Groups.Add(
            new SettingPageGroup(
                "Volo.Abp.Chat",
                l["Menu:Chat"],
                typeof(ChatAppSettingGroupViewComponent)
            )
        );
    }
} 