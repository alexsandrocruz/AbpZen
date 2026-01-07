using Sapienza.Cursos.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Sapienza.Cursos.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class CursosController : AbpControllerBase
    {
        protected CursosController()
        {
            LocalizationResource = typeof(CursosResource);
        }
    }
}