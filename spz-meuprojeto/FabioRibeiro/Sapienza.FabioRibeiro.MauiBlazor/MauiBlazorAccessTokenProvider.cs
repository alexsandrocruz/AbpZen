using Sapienza.FabioRibeiro.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Sapienza.FabioRibeiro.MauiBlazor;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class MauiBlazorAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly IFabioRibeiroApplicationSettingService _leptonXDemoAppApplicationSettingService;

    public MauiBlazorAccessTokenProvider(IFabioRibeiroApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        _leptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _leptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
    }
}
