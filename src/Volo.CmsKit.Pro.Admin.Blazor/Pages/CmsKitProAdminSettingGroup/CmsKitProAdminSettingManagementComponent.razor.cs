using System.Threading.Tasks;
using Blazorise;
using Microsoft.AspNetCore.Components;
using Volo.Abp.AspNetCore.Components.Messages;
using Volo.Abp.AspNetCore.Components.Web.Configuration;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Admin.Contact;
using Volo.CmsKit.GlobalFeatures;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKitProAdminSettingGroup;

public partial class CmsKitProAdminSettingManagementComponent
{
    [Inject]
    protected IContactSettingsAppService ContactSettingsAppService { get; set; }

    [Inject]
    protected INotificationService Notify { get; set; }

    [Inject]
    private ICurrentApplicationConfigurationCacheResetService CurrentApplicationConfigurationCacheResetService { get; set; }

    protected CmsKitProSettingsModel Settings;

    protected string SelectedTab = "Contact";

    protected Validations ContactSettingValidation;

    protected override async Task OnInitializedAsync()
    {
        if (!GlobalFeatureManager.Instance.IsEnabled<ContactFeature>())
        {
            return;
        }

        Settings = new CmsKitProSettingsModel
        {
            ContactSetting = await ContactSettingsAppService.GetAsync()
        };
    }

    protected virtual async Task UpdateContactSettingsAsync()
    {
        if (!await ContactSettingValidation.ValidateAll())
        {
            return;
        }

        await ContactSettingsAppService.UpdateAsync(new UpdateCmsKitContactSettingDto
        {
            ReceiverEmailAddress = Settings.ContactSetting.ReceiverEmailAddress
        });

        await CurrentApplicationConfigurationCacheResetService.ResetAsync();

        await Notify.Success(L["SavedSuccessfully"]);
    }
}

public class CmsKitProSettingsModel
{
    public CmsKitContactSettingDto ContactSetting { get; set; }
}
