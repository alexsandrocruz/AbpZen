using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Components.Web.Theming.Toolbars;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Chat.Authorization;
using Volo.Chat.Blazor.Components;

namespace Volo.Chat.Blazor;

public class ChatToolbarContributor : IToolbarContributor
{
    public async Task ConfigureToolbarAsync(IToolbarConfigurationContext context)
    {
        var featureChecker = context.ServiceProvider.GetRequiredService<IFeatureChecker>();

        if (context.Toolbar.Name == StandardToolbars.Main && await featureChecker.IsEnabledAsync(ChatFeatures.Enable))
        {
            var permissionChecker = context.ServiceProvider.GetRequiredService<IPermissionChecker>();
            if (await permissionChecker.IsGrantedAsync(ChatPermissions.Messaging))
            {
                context.Toolbar.Items.Add(new ToolbarItem(typeof(MessagesToolbarItem)));
            }
        }
    }
}
