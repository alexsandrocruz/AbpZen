using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Volo.Chat.Blazor.WebAssembly;

[DependsOn(
    typeof(ChatBlazorModule),
    typeof(ChatHttpApiClientModule),
    typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
)]
public class ChatBlazorWebAssemblyModule : AbpModule
{

}
