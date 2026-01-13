using Sapienza.EventoZen.MauiBlazor.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client.Authentication;

namespace Sapienza.EventoZen.MauiBlazor;

[Volo.Abp.DependencyInjection.Dependency(ReplaceServices = true)]
public class MauiBlazorAccessTokenProvider : IAbpAccessTokenProvider, ITransientDependency
{
    private readonly IEventoZenApplicationSettingService _leptonXDemoAppApplicationSettingService;

    public MauiBlazorAccessTokenProvider(IEventoZenApplicationSettingService leptonXDemoAppApplicationSettingService)
    {
        _leptonXDemoAppApplicationSettingService = leptonXDemoAppApplicationSettingService;
    }

    public async Task<string> GetTokenAsync()
    {
        return await _leptonXDemoAppApplicationSettingService.GetAccessTokenAsync();
    }
}
