using Sapienza.Cursos.Localization;
using Volo.Abp.AspNetCore.Components;

namespace Sapienza.Cursos.Blazor;

public abstract class CursosComponentBase : AbpComponentBase
{
    protected CursosComponentBase()
    {
        LocalizationResource = typeof(CursosResource);
    }
}
