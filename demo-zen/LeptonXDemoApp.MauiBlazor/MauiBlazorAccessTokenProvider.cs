using LeptonXDemoApp.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace LeptonXDemoApp.MauiBlazor;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class MauiBlazorAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly ILeptonXDemoAppApplicationSettingService _leptonXDemoAppApplicationSettingService;

    public MauiBlazorAccessTokenProvider(ILeptonXDemoAppApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        _leptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _leptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
    }
}
