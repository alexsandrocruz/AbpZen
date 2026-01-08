using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.Lexus
{
    [Dependency(ReplaceServices = true)]
    public class LexusBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.Lexus";
    }
}
