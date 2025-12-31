using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Saas.Host.Dtos;

namespace Volo.Saas.Host.Pages.Saas.Host.Tenants;

public class EditModalModel : SaasHostPageModel
{
    [BindProperty]
    public TenantInfoModel Tenant { get; set; }

    protected ITenantAppService TenantAppService { get; }
    protected IEditionAppService EditionAppService { get; }

    public List<SelectListItem> EditionsComboboxItems { get; set; } = new List<SelectListItem>();

    public bool HasSubscription { get; protected set; }

    public EditModalModel(ITenantAppService tenantAppService, IEditionAppService editionAppService)
    {
        TenantAppService = tenantAppService;
        EditionAppService = editionAppService;
    }

    public virtual async Task OnGetAsync(Guid id)
    {
        var tenantDto = await TenantAppService.GetAsync(id);

        HasSubscription = tenantDto.EditionEndDateUtc > DateTime.UtcNow;

        Tenant = ObjectMapper.Map<SaasTenantDto, TenantInfoModel>(tenantDto);

        var editions = await EditionAppService.GetAllListAsync();

        EditionsComboboxItems.Add(new SelectListItem(L["NotAssigned"].Value, ""));
        EditionsComboboxItems.AddRange(editions.Select(e => new SelectListItem(e.DisplayName, e.Id.ToString(), Tenant.EditionId == e.Id)).ToList());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<TenantInfoModel, SaasTenantUpdateDto>(Tenant);
        await TenantAppService.UpdateAsync(Tenant.Id, input);

        return NoContent();
    }

    public class TenantInfoModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [SelectItems(nameof(EditionsComboboxItems))]
        public Guid? EditionId { get; set; }

        [Required]
        [DynamicStringLength(typeof(TenantConsts),nameof(TenantConsts.MaxNameLength))]
        public string Name { get; set; }

        public TenantActivationState ActivationState { get; set; }

        public DateTime? ActivationEndDate { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
