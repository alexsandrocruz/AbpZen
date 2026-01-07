using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Cursos.Web
{
    [Dependency(ReplaceServices = true)]
    public class CursosBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Cursos";
    }
}
