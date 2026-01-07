using Sapienza.Cursos.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Cursos.MauiBlazor;

public abstract class CursosComponentBase : AbpComponentBase
{
    protected CursosComponentBase()
    {
        LocalizationResource = typeof(CursosResource);
    }
}
