using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace Volo.Payment.Plans;

public class PlanAppService_Tests : PaymentTestBase<PaymentApplicationTestModule>
{
    private IPlanAppService planAppService;
    private PaymentTestData testData;

    public PlanAppService_Tests()
    {
        planAppService = GetRequiredService<IPlanAppService>();
        testData = GetRequiredService<PaymentTestData>();
    }

    [Fact]
    public async Task GetGatewayPlanAsync_ShouldWorkProperly()
    {
        var gatewayPlan = await planAppService.GetGatewayPlanAsync(testData.Plan_1_Id, testData.GatewayPlan_1_Gateway);

        gatewayPlan.ShouldNotBeNull();
        gatewayPlan.Gateway.ShouldBe(testData.GatewayPlan_1_Gateway);
    }

    [Fact]
    public async Task GetGatewayPlanAsync_ShouldThrowEntityNotFoundException_WithWrongGateway()
    {
        var nonExistingGateway = "Some-Random-Gateway";
        var exception = await Should.ThrowAsync<EntityNotFoundException>(planAppService.GetGatewayPlanAsync(testData.Plan_1_Id, nonExistingGateway));

        exception.ShouldNotBeNull();
        exception.EntityType.ShouldBe(typeof(GatewayPlan));
    }

    [Fact]
    public async Task GetPlanListAsync_ShouldWorkProperly()
    {
        var planList = await planAppService.GetPlanListAsync();

        planList.ShouldNotBeEmpty();
        planList.Count.ShouldBe(2);
    }
}
