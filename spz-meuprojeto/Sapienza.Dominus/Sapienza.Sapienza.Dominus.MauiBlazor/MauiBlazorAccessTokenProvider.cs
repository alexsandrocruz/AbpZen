using Sapienza.Dominus.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Sapienza.Dominus.MauiBlazor;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class MauiBlazorAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly IDominusApplicationSettingService _leptonXDemoAppApplicationSettingService;

    public MauiBlazorAccessTokenProvider(IDominusApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        _leptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _leptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
    }
}
