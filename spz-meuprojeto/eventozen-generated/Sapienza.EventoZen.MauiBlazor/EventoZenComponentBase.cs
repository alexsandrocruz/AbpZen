using Sapienza.EventoZen.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.EventoZen.MauiBlazor;

public abstract class EventoZenComponentBase : AbpComponentBase
{
    protected EventoZenComponentBase()
    {
        LocalizationResource = typeof(EventoZenResource);
    }
}
