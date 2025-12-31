using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Account.Settings;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.SettingManagement;

namespace Volo.Abp.Account.ExternalLogins;

public class ExternalLoginPasswordVerifiedHelper : ITransientDependency
{
    private readonly ISettingManager _settingManager;
    private readonly IJsonSerializer _jsonSerializer;

    public ExternalLoginPasswordVerifiedHelper(
        ISettingManager settingManager,
        IJsonSerializer jsonSerializer)
    {
        _settingManager = settingManager;
        _jsonSerializer = jsonSerializer;
    }

    public virtual async Task<bool> HasPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await _settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        if (settingString.IsNullOrWhiteSpace())
        {
            return false;
        }

        var settings = _jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);
        return settings.Any(s => s.LoginProvider == loginProvider && s.ProviderKey == providerKey);
    }

    public virtual async Task SetPasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await _settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        var settings = settingString.IsNullOrWhiteSpace()
            ? new List<ExternalLoginSetting>()
            : _jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);

        settings.RemoveAll(s => s.LoginProvider == loginProvider && s.ProviderKey == providerKey);
        settings.Add(new ExternalLoginSetting(loginProvider, providerKey));

        await _settingManager.SetForUserAsync(
            userId,
            AccountSettingNames.ExternalLoginPasswordVerified,
            _jsonSerializer.Serialize(settings),
            forceToSet: true
        );
    }

    public virtual async Task RemovePasswordVerifiedAsync(Guid userId, string loginProvider, string providerKey)
    {
        var settingString = await _settingManager.GetOrNullForUserAsync(
            AccountSettingNames.ExternalLoginPasswordVerified,
            userId,
            fallback: false);

        if (settingString.IsNullOrWhiteSpace())
        {
            return;
        }

        var settings = _jsonSerializer.Deserialize<List<ExternalLoginSetting>>(settingString);
        settings.RemoveAll(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);

        await _settingManager.SetForUserAsync(
            userId,
            AccountSettingNames.ExternalLoginPasswordVerified,
            _jsonSerializer.Serialize(settings),
            forceToSet: true
        );
    }
}
