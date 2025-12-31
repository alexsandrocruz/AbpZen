using System;
using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Volo.Abp.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.Features;
using Volo.Abp.Identity.Features;
using Volo.Abp.Identity.Localization;
using Volo.Abp.Ldap.Localization;

namespace Volo.Abp.Identity.Pro.Blazor.Pages.Identity.IdentitySettingGroup;

public partial class IdentitySettingManagementComponent : AbpComponentBase
{
    [Inject]
    protected IIdentitySettingsAppService IdentitySettingsAppService { get; set; }

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    [Inject]
    protected IUiMessageService UiMessageService { get; set; }

    [Inject]
    protected IStringLocalizer<IdentityResource> L { get; set; }

    [Inject]
    protected IStringLocalizer<LdapResource> LdapLocalizer { get; set; }

    [Inject]
    protected IFeatureChecker FeatureChecker { get; set; }

    protected string SelectedTab = "IdentitySettingsGeneral";

    protected IdentitySettingViewModel Settings;

    protected Validations IdentitySettingValidation;

    protected Validations IdentityLdapSettingsValidations;

    protected Validations IdentityOAuthSettingsValidations;

    protected async override Task OnInitializedAsync()
    {
        Settings = new IdentitySettingViewModel
        {
            IdentitySettings =  await IdentitySettingsAppService.GetAsync()
        };

        if (await FeatureChecker.IsEnabledAsync(IdentityProFeature.EnableLdapLogin))
        {
            Settings.IdentityLdapSettings = await IdentitySettingsAppService.GetLdapAsync();
        }

        if (await FeatureChecker.IsEnabledAsync(IdentityProFeature.EnableOAuthLogin))
        {
            Settings.IdentityOAuthSettings = await IdentitySettingsAppService.GetOAuthAsync();
        }

        Settings.IdentitySessionSettings = await IdentitySettingsAppService.GetSessionAsync();
    }

    protected virtual async Task UpdateSettingsAsync()
    {
        try
        {
            await IdentitySettingsAppService.UpdateAsync(Settings.IdentitySettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateOAuthSettings()
    {
        try
        {
            await IdentitySettingsAppService.UpdateOAuthAsync(Settings.IdentityOAuthSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateLdapSettings()
    {
        try
        {
            await IdentitySettingsAppService.UpdateLdapAsync(Settings.IdentityLdapSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }

    protected virtual async Task UpdateSessionsSettings()
    {
        try
        {
            await IdentitySettingsAppService.UpdateSessionAsync(Settings.IdentitySessionSettings);

            await CurrentApplicationConfigurationCacheResetService.ResetAsync();

            await Notify.Success(L["SavedSuccessfully"]);
        }
        catch (Exception ex)
        {
            await HandleErrorAsync(ex);
        }
    }
}

public class IdentitySettingViewModel
{
    public IdentitySettingsDto IdentitySettings { get; set; }

    public IdentityLdapSettingsDto IdentityLdapSettings { get; set; }

    public IdentityOAuthSettingsDto IdentityOAuthSettings { get; set; }

    public IdentitySessionSettingsDto IdentitySessionSettings { get; set; }
}
