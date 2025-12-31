using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Payment.Plans;
using Volo.Payment.Stripe;

namespace Volo.Payment.DemoApp;

public class DemoAppDataSeeder : IDataSeedContributor, ITransientDependency
{
    protected IPlanRepository PlanRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public DemoAppDataSeeder(IPlanRepository planRepository, IGuidGenerator guidGenerator)
    {
        PlanRepository = planRepository;
        GuidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await PlanRepository.FindAsync(DemoAppData.PlanId) == null)
        {
            var plan = await PlanRepository.InsertAsync(new Plan(DemoAppData.PlanId, DemoAppData.PlanName), autoSave: true);

            await PlanRepository.InsertGatewayPlanAsync(
                new GatewayPlan(
                    plan.Id,
                    StripeConsts.GatewayName,
                    "price_1IyaejDnWM6DfssS8rru3KbC"));

            var plan2 = await PlanRepository.InsertAsync(new Plan(DemoAppData.Plan_2_Id, DemoAppData.Plan_2_Name), autoSave: true);

            await PlanRepository.InsertGatewayPlanAsync(
                new GatewayPlan(
                    plan2.Id,
                    StripeConsts.GatewayName,
                    "price_1IyaftDnWM6DfssSMrg1bnRi"));
        }
    }
}
