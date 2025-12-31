using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Web.Pages.Payment.Plans;

public class UpdateModalModel : PaymentPageModel
{
    protected IPlanAdminAppService PlanAdminAppService { get; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    [BindProperty]
    public PlanUpdateViewModel ViewModel { get; set; }

    public UpdateModalModel(IPlanAdminAppService planAdminAppService)
    {
        PlanAdminAppService = planAdminAppService;
    }

    public virtual async Task OnGetAsync()
    {
        var planDto = await PlanAdminAppService.GetAsync(Id);
        ViewModel = ObjectMapper.Map<PlanDto, PlanUpdateViewModel>(planDto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();
        
        var input = ObjectMapper.Map<PlanUpdateViewModel, PlanUpdateInput>(ViewModel);
        await PlanAdminAppService.UpdateAsync(Id, input);

        return NoContent();
    }

    public class PlanUpdateViewModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [Required]
        [DynamicMaxLength(typeof(PlanConsts), nameof(PlanConsts.MaxNameLength))]
        public string Name { get; set; }

        [HiddenInput]
        public string ConcurrencyStamp { get; set; }
    }
}
