using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Sapienza.FabioRibeiro.Web
{
    [Dependency(ReplaceServices = true)]
    public class FabioRibeiroBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.FabioRibeiro";
    }
}
