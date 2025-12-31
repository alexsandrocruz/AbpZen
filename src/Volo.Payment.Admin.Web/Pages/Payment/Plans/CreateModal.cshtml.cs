using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Web.Pages.Payment.Plans;

public class CreateModalModel : PaymentPageModel
{
    protected IPlanAdminAppService PlanAdminAppService { get; }

    [BindProperty]
    public PlanCreateViewModel ViewModel { get; set; }

    public CreateModalModel(IPlanAdminAppService planAdminAppService)
    {
        PlanAdminAppService = planAdminAppService;
    }

    public virtual Task<IActionResult> OnGetAsync()
    {
        ViewModel = new PlanCreateViewModel();
        return Task.FromResult<IActionResult>(Page());
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        ValidateModel();
        
        var input = ObjectMapper.Map<PlanCreateViewModel, PlanCreateInput>(ViewModel);
        await PlanAdminAppService.CreateAsync(input);

        return NoContent();
    }

    public class PlanCreateViewModel : ExtensibleObject
    {
        [Required]
        [DynamicMaxLength(typeof(PlanConsts), nameof(PlanConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
