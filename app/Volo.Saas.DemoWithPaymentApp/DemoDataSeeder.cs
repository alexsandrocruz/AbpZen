using System.Threading.Tasks;
using Volo.Abp.Account.Web;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Payment.Plans;
using Volo.Payment.Stripe;
using Volo.Saas.Editions;

namespace Volo.Saas.DemoWithPaymentApp;

public class DemoDataSeeder : IDataSeedContributor, ITransientDependency
{
    protected IPlanRepository PlanRepository { get; }
    protected IEditionRepository EditionRepository { get; }
    protected IGuidGenerator GuidGenerator { get; }

    public DemoDataSeeder(
        IPlanRepository planRepository,
        IEditionRepository editionRepository,
        IGuidGenerator guidGenerator)
    {
        PlanRepository = planRepository;
        EditionRepository = editionRepository;
        GuidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        var plan1 = await PlanRepository.FindAsync(DemoData.Plan_1_Id);
        if (plan1 == null)
        {
            plan1 = await PlanRepository.InsertAsync(new Plan(DemoData.Plan_1_Id, DemoData.Plan_1_Name), autoSave: true);

            await PlanRepository.InsertGatewayPlanAsync(
                new GatewayPlan(
                    plan1.Id,
                    StripeConsts.GatewayName,
                    "price_1IyaejDnWM6DfssS8rru3KbC"));
        }

        if (await EditionRepository.FindAsync(DemoData.Edition_1_Id) == null)
        {
            await EditionRepository.InsertAsync(new Edition(DemoData.Edition_1_Id, DemoData.Edition_1_Name + " Edition")
            {
                PlanId = plan1.Id
            });
        }

        var plan2 = await PlanRepository.FindAsync(DemoData.Plan_2_Id);
        if (plan2 == null)
        {
            plan2 = await PlanRepository.InsertAsync(new Plan(DemoData.Plan_2_Id, DemoData.Plan_2_Name), autoSave: true);

            await PlanRepository.InsertGatewayPlanAsync(
                new GatewayPlan(
                    plan2.Id,
                    StripeConsts.GatewayName,
                    "price_1IyaftDnWM6DfssSMrg1bnRi"));
        }

        if (await EditionRepository.FindAsync(DemoData.Edition_2_Id) == null)
        {
            await EditionRepository.InsertAsync(new Edition(DemoData.Edition_2_Id, DemoData.Edition_2_Name + " Edition")
            {
                PlanId = plan2.Id
            });
        }
    }
}
