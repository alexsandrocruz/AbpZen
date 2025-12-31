using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.SettingManagement;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;

namespace Volo.CmsKit.Admin.Contact;

[RequiresFeature(CmsKitProFeatures.ContactEnable)]
[RequiresGlobalFeature(ContactFeature.Name)]
[Authorize(CmsKitProAdminPermissions.Contact.SettingManagement)]
public class ContactSettingsAppService : CmsKitProAdminAppService, IContactSettingsAppService
{
    protected ISettingManager SettingManager { get; }

    public ContactSettingsAppService(ISettingManager settingManager)
    {
        SettingManager = settingManager;
    }

    public virtual async Task<CmsKitContactSettingDto> GetAsync()
    {
        var receiverEmailAddress = await SettingManager.GetOrNullForCurrentTenantAsync(CmsKitProSettingNames.Contact.ReceiverEmailAddress);

        return new CmsKitContactSettingDto
        {
            ReceiverEmailAddress = receiverEmailAddress
        };
    }

    public virtual async Task UpdateAsync(UpdateCmsKitContactSettingDto input)
    {
        await SettingManager.SetForCurrentTenantAsync(CmsKitProSettingNames.Contact.ReceiverEmailAddress, input.ReceiverEmailAddress);
    }
}
