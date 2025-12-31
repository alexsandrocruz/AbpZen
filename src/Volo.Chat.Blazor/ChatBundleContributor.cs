using Volo.Abp.Bundling;

namespace Volo.Chat.Blazor;

public class ChatBundleContributor: IBundleContributor
{
    public void AddScripts(BundleContext context)
    {
        context.Add("_content/Volo.Chat.Blazor/libs/AvatarManager.js");
    }

    public void AddStyles(BundleContext context)
    {

    }
}
