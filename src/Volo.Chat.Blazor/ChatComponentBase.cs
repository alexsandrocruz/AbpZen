using Volo.Chat.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Volo.Chat.Blazor;

public abstract class ChatComponentBase : AbpComponentBase
{
    protected ChatComponentBase()
    {
        LocalizationResource = typeof(ChatResource);
    }
}
