using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

public class AccountProfilePersonalInfoManagementGroupViewComponent : AbpViewComponent
{
    protected IProfileAppService ProfileAppService { get; }

    public AccountProfilePersonalInfoManagementGroupViewComponent(IProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;

        ObjectMapperContext = typeof(AbpAccountPublicWebModule);
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await ProfileAppService.GetAsync();

        var model = ObjectMapper.Map<ProfileDto, PersonalInfoModel>(user);
        if (model.SupportsMultipleTimezone)
        {
            model.TimeZoneItems = (await ProfileAppService.GetTimezonesAsync()).Select(x => new SelectListItem(x.Name, x.Value)).ToList();
        }

        return View("~/Pages/Account/Components/ProfileManagementGroup/PersonalInfo/Default.cshtml", model);
    }

    public class PersonalInfoModel : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        [Display(Name = "DisplayName:UserName")]
        public string UserName { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [Display(Name = "DisplayName:Email")]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        [Display(Name = "DisplayName:Name")]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        [Display(Name = "DisplayName:Surname")]
        public string Surname { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [Display(Name = "DisplayName:PhoneNumber")]
        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool SupportsMultipleTimezone { get; set; }

        public string Timezone { get; set; }

        public List<SelectListItem> TimeZoneItems { get; set; }
    }
}
