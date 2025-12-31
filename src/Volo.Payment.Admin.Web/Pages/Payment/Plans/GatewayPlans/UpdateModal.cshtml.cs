using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Gateways;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Web.Pages.Payment.Plans.GatewayPlans;

public class UpdateModalModel : PaymentPageModel
{
    public List<SelectListItem> SelectableGateways { get; protected set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid PlanId { get; set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public string Gateway { get; set; }

    [BindProperty]
    public GatewayPlansUpdateViewModel ViewModel { get; set; }

    protected IPlanAdminAppService PlanAdminAppService { get; }

    protected IPlanAppService PlanAppService { get; }

    protected IGatewayAppService GatewayAppService { get; }

    private IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    public UpdateModalModel(
        IPlanAdminAppService planAdminAppService,
        IPlanAppService planAppService,
        IGatewayAppService gatewayAppService,
        IOptions<PaymentWebOptions> paymentWebOptions)
    {
        PlanAdminAppService = planAdminAppService;
        PlanAppService = planAppService;
        GatewayAppService = gatewayAppService;
        PaymentWebOptions = paymentWebOptions;
    }

    public virtual async Task OnGetAsync()
    {
        var subscriptionSupportedGateways = await GatewayAppService.GetSubscriptionSupportedGatewaysAsync();
        SelectableGateways = subscriptionSupportedGateways
            .Select(g => new SelectListItem(g.Name, g.DisplayName))
            .ToList();

        var gatewayPlanDto = await PlanAppService.GetGatewayPlanAsync(PlanId, Gateway);
        ViewModel = ObjectMapper.Map<GatewayPlanDto, GatewayPlansUpdateViewModel>(gatewayPlanDto);
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var input = ObjectMapper.Map<GatewayPlansUpdateViewModel, GatewayPlanUpdateInput>(ViewModel);

        await PlanAdminAppService.UpdateGatewayPlanAsync(PlanId, Gateway, input);

        return NoContent();
    }

    public class GatewayPlansUpdateViewModel : ExtensibleObject
    {
        [Required]
        public string ExternalId { get; set; }
    }
}
