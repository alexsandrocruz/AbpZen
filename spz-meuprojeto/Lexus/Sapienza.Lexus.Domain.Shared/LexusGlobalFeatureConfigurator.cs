using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace Sapienza.Lexus
{
    public static class LexusGlobalFeatureConfigurator
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
