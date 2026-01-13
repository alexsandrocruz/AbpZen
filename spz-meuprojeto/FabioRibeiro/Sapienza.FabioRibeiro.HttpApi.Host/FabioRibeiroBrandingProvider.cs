using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Sapienza.FabioRibeiro
{
    [Dependency(ReplaceServices = true)]
    public class FabioRibeiroBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Sapienza.FabioRibeiro";
    }
}
