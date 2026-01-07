using Sapienza.Cursos.Localization;
using Volo.Abp.Application.Services;

namespace Sapienza.Cursos
{
    /* Inherit your application services from this class.
     */
    public abstract class CursosAppService : ApplicationService
    {
        protected CursosAppService()
        {
            LocalizationResource = typeof(CursosResource);
        }
    }
}
