using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.Lexus.Web
{
    [Dependency(ReplaceServices = true)]
    public class LexusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Lexus";
    }
}
