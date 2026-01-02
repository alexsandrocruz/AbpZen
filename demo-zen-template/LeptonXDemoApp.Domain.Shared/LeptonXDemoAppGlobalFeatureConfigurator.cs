using Volo.Abp.GlobalFeatures;
using Volo.Abp.Threading;

namespace LeptonXDemoApp
{
    public static class LeptonXDemoAppGlobalFeatureConfigurator
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
