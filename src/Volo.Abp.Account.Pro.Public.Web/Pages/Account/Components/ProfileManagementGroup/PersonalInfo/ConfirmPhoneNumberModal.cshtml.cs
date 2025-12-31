using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Identity.Settings;
using Volo.Abp.Settings;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

public class ConfirmPhoneNumberModalModel : AccountPageModel
{
    [BindProperty]
    [Required]
    public string PhoneConfirmationToken { get; set; }


    public virtual async Task OnGetAsync()
    {
        if (!await SettingProvider.GetAsync(IdentitySettingNames.SignIn.EnablePhoneNumberConfirmation, true))
        {
            throw new BusinessException("Volo.Account:PhoneNumberConfirmationDisabled"); //TODO: Localize!
        }

        await AccountAppService.SendPhoneNumberConfirmationTokenAsync(new SendPhoneNumberConfirmationTokenDto
        {
            UserId = CurrentUser.GetId()
        });
    }

    public virtual async Task OnPostAsync()
    {
        await AccountAppService.ConfirmPhoneNumberAsync(new ConfirmPhoneNumberInput
        {
            UserId = CurrentUser.GetId(),
            Token = PhoneConfirmationToken
        });
    }
}
