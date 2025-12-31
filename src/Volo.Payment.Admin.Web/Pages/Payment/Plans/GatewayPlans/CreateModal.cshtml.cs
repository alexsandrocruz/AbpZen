using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Volo.Abp.ObjectExtending;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Gateways;

namespace Volo.Payment.Admin.Web.Pages.Payment.Plans.GatewayPlans;

public class CreateModalModel : PaymentPageModel
{
    public List<SelectListItem> SelectableGateways { get; protected set; }

    [HiddenInput]
    [BindProperty(SupportsGet = true)]
    public Guid PlanId { get; set; }

    [BindProperty]
    public GatewayPlanCreateViewModel ViewModel { get; set; }

    protected IPlanAdminAppService PlanAdminAppService { get; }

    protected IGatewayAppService GatewayAppService { get; }

    private IOptions<PaymentWebOptions> PaymentWebOptions { get; }

    public CreateModalModel(
        IPlanAdminAppService planAdminAppService,
        IGatewayAppService gatewayAppService,
        IOptions<PaymentWebOptions> paymentWebOptions)
    {
        PlanAdminAppService = planAdminAppService;
        GatewayAppService = gatewayAppService;
        PaymentWebOptions = paymentWebOptions;
    }

    public virtual async Task OnGetAsync()
    {
        ViewModel = new GatewayPlanCreateViewModel();
        var subscriptionSupportedGateways = await GatewayAppService.GetSubscriptionSupportedGatewaysAsync();
        SelectableGateways = subscriptionSupportedGateways
            .Select(g => new SelectListItem(g.Name, g.DisplayName))
            .ToList();
    }

    public virtual async Task<IActionResult> OnPostAsync()
    {
        var input = ObjectMapper.Map<GatewayPlanCreateViewModel, GatewayPlanCreateInput>(ViewModel);
        await PlanAdminAppService.CreateGatewayPlanAsync(PlanId, input);

        return NoContent();
    }

    public class GatewayPlanCreateViewModel : ExtensibleObject
    {
        [Required]
        [SelectItems(nameof(SelectableGateways))]
        public string Gateway { get; set; }

        [Required]
        public string ExternalId { get; set; }
    }
}
