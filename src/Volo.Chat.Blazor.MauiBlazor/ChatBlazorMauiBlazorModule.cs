using Volo.Abp.AspNetCore.Components.MauiBlazor.Theming;
using Volo.Abp.Modularity;

namespace Volo.Chat.Blazor.MauiBlazor;

[DependsOn(
    typeof(ChatBlazorModule),
    typeof(ChatHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsMauiBlazorThemingModule)
)]
public class ChatBlazorMauiBlazorModule : AbpModule
{
    
    
}