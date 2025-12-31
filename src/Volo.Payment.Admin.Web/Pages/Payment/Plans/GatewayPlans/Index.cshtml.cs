using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Web.Pages.Payment.Plans.GatewayPlans;

public class IndexModel : PaymentPageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid Id { get; set; }

    public PlanDto Plan { get; set; }

    protected IPlanAdminAppService PlanAdminAppService { get; }

    public IndexModel(IPlanAdminAppService planAdminAppService)
    {
        PlanAdminAppService = planAdminAppService;
    }

    public virtual async Task OnGetAsync()
    {
        Plan = await PlanAdminAppService.GetAsync(Id);
    }
}
