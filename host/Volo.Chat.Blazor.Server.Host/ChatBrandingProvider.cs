using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Volo.Chat.Blazor.Server.Host;

[Dependency(ReplaceServices = true)]
public class ChatBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Chat";
}
