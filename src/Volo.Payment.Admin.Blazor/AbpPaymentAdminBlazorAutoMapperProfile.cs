using AutoMapper;
using Volo.Payment.Admin.Plans;
using Volo.Payment.Plans;

namespace Volo.Payment.Admin.Blazor;

public class AbpPaymentAdminBlazorAutoMapperProfile : Profile
{
    public AbpPaymentAdminBlazorAutoMapperProfile()
    {
        CreateMap<PlanDto, PlanUpdateInput>()
            .MapExtraProperties();

        CreateMap<GatewayPlanDto, GatewayPlanUpdateInput>()
            .MapExtraProperties();
    }
}
