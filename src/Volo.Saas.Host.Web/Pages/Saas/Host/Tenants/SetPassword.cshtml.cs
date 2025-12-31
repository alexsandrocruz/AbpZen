using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Saas.Host.Dtos;
using Volo.Saas.Localization;

namespace Volo.Saas.Host.Pages.Saas.Host.Tenants;

public class SetPassword : SaasHostPageModel
{
    [BindProperty] public ChangeTenantPasswordViewModel ChangePasswordInput { get; set; }
    [BindProperty] public string TenantName { get; set; }

    public ITenantAppService TenantAppService { get; }

    public SetPassword(ITenantAppService tenantAppService)
    {
        LocalizationResourceType = typeof(SaasResource);
        TenantAppService = tenantAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var tenant = await TenantAppService.GetAsync(id);
        TenantName = tenant.Name;
        ChangePasswordInput = new ChangeTenantPasswordViewModel { Id = id };
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        await TenantAppService.SetPasswordAsync(ChangePasswordInput.Id, new SaasTenantSetPasswordDto()
        {
            Password = ChangePasswordInput.NewPassword,
            Username = ChangePasswordInput.UserName
        });

        return NoContent();
    }

    public class ChangeTenantPasswordViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        public string UserName { get; set; } = TenantConsts.Username;
    }
}
