using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Volo.Abp.LeptonX.Shared;

namespace Volo.Abp.AspNetCore.Components.Web.LeptonXTheme.Components.ApplicationLayout;

public partial class SideMenuLayout
{
    [Inject]
    protected IAbpUtilsService UtilsService { get; set; }

    [Inject]
    IJSRuntime JSRuntime { get; set; }

    [Inject]
    protected IOptions<LeptonXThemeOptions> Options { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await UtilsService.AddClassToTagAsync("body", GetBodyClassName());
            await JSRuntime.InvokeVoidAsync("initLeptonX", new[] { "side-menu", Options.Value.DefaultStyle });
            await JSRuntime.InvokeVoidAsync("afterLeptonXInitialization", new[] { "side-menu", Options.Value.DefaultStyle });
            
        }
    }

    private string GetBodyClassName()
    {
        return "lpx-theme-" + Options.Value.DefaultStyle;
    }
}
