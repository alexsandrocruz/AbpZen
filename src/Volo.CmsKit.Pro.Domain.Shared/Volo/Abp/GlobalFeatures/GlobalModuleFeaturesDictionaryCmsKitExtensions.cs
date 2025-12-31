using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.Abp.GlobalFeatures;

public static class GlobalModuleFeaturesDictionaryCmsKitExtensions
{
    public static GlobalCmsKitProFeatures CmsKitPro(
        [NotNull] this GlobalModuleFeaturesDictionary modules)
    {
        Check.NotNull(modules, nameof(modules));

        return modules
                .GetOrAdd(
                    GlobalCmsKitProFeatures.ModuleName,
                    _ => new GlobalCmsKitProFeatures(modules.FeatureManager)
                )
            as GlobalCmsKitProFeatures;
    }

    public static GlobalModuleFeaturesDictionary CmsKitPro(
        [NotNull] this GlobalModuleFeaturesDictionary modules,
        [NotNull] Action<GlobalCmsKitProFeatures> configureAction)
    {
        Check.NotNull(configureAction, nameof(configureAction));

        configureAction(modules.CmsKitPro());

        return modules;
    }
}
