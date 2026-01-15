using Sapienza.Sapienza.Dominus.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Sapienza.Sapienza.Dominus.MauiBlazor;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class MauiBlazorAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly ISapienza.DominusApplicationSettingService _leptonXDemoAppApplicationSettingService;

    public MauiBlazorAccessTokenProvider(ISapienza.DominusApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        _leptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _leptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
    }
}
