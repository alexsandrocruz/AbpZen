using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Settings;

namespace Volo.Abp.Identity.Settings;

public static class IdentityProPreventConcurrentLoginBehaviourSettingHelper
{
    public async static Task<IdentityProPreventConcurrentLoginBehaviour> Get([NotNull] ISettingProvider settingProvider)
    {
        Check.NotNull(settingProvider, nameof(settingProvider));

        var value = await settingProvider.GetOrNullAsync(IdentityProSettingNames.Session.PreventConcurrentLogin);
        if (value.IsNullOrWhiteSpace() || !Enum.TryParse<IdentityProPreventConcurrentLoginBehaviour>(value, out var behaviour))
        {
            throw new AbpException($"{IdentityProSettingNames.Session.PreventConcurrentLogin} setting value is invalid");
        }

        return behaviour;
    }
}
