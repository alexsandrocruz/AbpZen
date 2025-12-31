using Shouldly;
using System;
using System.Threading.Tasks;
using Volo.Payment.Admin.Plans;
using Xunit;

namespace Volo.Payment.Plans;

public class PlanAdminAppService_Tests : PaymentTestBase<PaymentApplicationTestModule>
{
    private IPlanAdminAppService planAdminAppService;
    private IPlanRepository planRepository;
    private PaymentTestData testData;

    public PlanAdminAppService_Tests()
    {
        planAdminAppService = GetRequiredService<IPlanAdminAppService>();
        planRepository = GetRequiredService<IPlanRepository>();
        testData = GetRequiredService<PaymentTestData>();
    }

    [Fact]
    public async Task CreateAsync_ShouldWorkProperly_WithCorrectData()
    {
        var planName = "Team Plan";
        var input = new PlanCreateInput
        {
            Name = planName
        };

        var planDto = await planAdminAppService.CreateAsync(input);

        planDto.Id.ShouldNotBe(Guid.Empty);

        var plan = await planRepository.GetAsync(planDto.Id);

        plan.Name.ShouldBe(planName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldWorkProperly_WithCorrectData()
    {
        var newPlanName = "Enterprise Plan";
        var updatedPlanDto = await planAdminAppService.UpdateAsync(testData.Plan_1_Id, new PlanUpdateInput
        {
            Name = newPlanName
        });

        updatedPlanDto.ShouldNotBeNull();

        var plan = await planRepository.GetAsync(testData.Plan_1_Id);
        plan.Name.ShouldBe(newPlanName);
    }
}
