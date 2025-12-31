using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;
using Volo.Abp.AspNetCore.Mvc.UI.Packages.SignalR;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Features;
using Volo.Abp.Modularity;
using Volo.Chat.Authorization;

namespace Volo.Chat.Web.Bundling;

[DependsOn(
    typeof(SignalRBrowserScriptContributor)
)]
public class ChatGlobalScriptContributor : BundleContributor
{
    public async override Task ConfigureBundleAsync(BundleConfigurationContext context)
    {
        var featureChecker = context.ServiceProvider.GetService<IFeatureChecker>();
        var permissionChecker = context.ServiceProvider.GetService<IPermissionChecker>();

        if (
            !await featureChecker!.IsEnabledAsync(ChatFeatures.Enable) ||
            !await permissionChecker!.IsGrantedAsync(ChatPermissions.Messaging)
            )
        {
            return;
        }
        
        context.Files.AddIfNotContains("/client-proxies/chat-proxy.js");
        context.Files.AddIfNotContains("/Pages/Chat/chatMessageReceiving.js");
    }
}
