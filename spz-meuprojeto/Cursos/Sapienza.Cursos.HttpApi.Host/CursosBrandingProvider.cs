using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Cursos
{
    [Dependency(ReplaceServices = true)]
    public class CursosBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Cursos";
    }
}
