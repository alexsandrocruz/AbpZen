using AutoMapper;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Admin.Web.Pages.Payment.Plans.GatewayPlans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Web;

public class PaymentAdminWebAutoMapperProfile : Profile
{
    public PaymentAdminWebAutoMapperProfile()
    {
        CreateMap<CreateModalModel.GatewayPlanCreateViewModel, GatewayPlanCreateInput>()
            .MapExtraProperties();

        CreateMap<GatewayPlanDto, UpdateModalModel.GatewayPlansUpdateViewModel>()
            .MapExtraProperties();

        CreateMap<UpdateModalModel.GatewayPlansUpdateViewModel, GatewayPlanUpdateInput>()
            .MapExtraProperties();

        CreateMap<Pages.Payment.Plans.CreateModalModel.PlanCreateViewModel, PlanCreateInput>()
            .MapExtraProperties();

        CreateMap<PlanDto, Pages.Payment.Plans.UpdateModalModel.PlanUpdateViewModel>()
            .MapExtraProperties();

        CreateMap<Pages.Payment.Plans.UpdateModalModel.PlanUpdateViewModel, PlanUpdateInput>()
            .MapExtraProperties();
    }
}
