using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace Sapienza.Sapienza.Dominus
{
    public static class Sapienza.DominusGlobalFeatureConfigurator
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
                {
                    cmsKit.EnableAll();
                });
            });
            
            OneTimeRunner.Run(() =>
            {
                GlobalFeatureManager.Instance.Modules.CmsKitPro(cmsKit =>
                {
                    cmsKit.EnableAll();
                });
            });
        }
    }
}
