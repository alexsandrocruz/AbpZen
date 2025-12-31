using Shouldly;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Guids;
using Volo.Abp.Modularity;
using Volo.Abp.Uow;
using Xunit;

namespace Volo.Payment.Plans;

public abstract class PlanRepository_Test<TStartupModule> : PaymentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly PaymentTestData testData;
    private readonly IPlanRepository planRepository;
    private readonly IGuidGenerator guidGenerator;
    public PlanRepository_Test()
    {
        testData = GetRequiredService<PaymentTestData>();
        planRepository = GetRequiredService<IPlanRepository>();
        guidGenerator = GetRequiredService<IGuidGenerator>();
    }

    [Fact]
    public async Task InsertAsync_ShouldWorkProperly_WithoutGatewayPlan()
    {
        var id = guidGenerator.Create();

        await planRepository.InsertAsync(
            new Plan(id, "Enterprise Pro Plan"), autoSave: true);

        var plan = await planRepository.FindAsync(id);

        plan.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetListAsync_ShouldWorkProperly()
    {
        var list = await planRepository.GetListAsync();

        list.ShouldNotBeEmpty();
        list.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetAsync_ShouldWorkProperly()
    {
        var plan = await planRepository.GetAsync(testData.Plan_1_Id);

        plan.ShouldNotBeNull();
    }

    [Fact]
    public async Task InsertGatewayPlanAsync_ShouldWorkProperly_WithNewlyCreatedPlan()
    {
        var id = guidGenerator.Create();
        var gateway = "iyzico";
        var externalId = "A1029384756Z";

        var plan = new Plan(id, "Enterprise Pro Plan");

        await planRepository.InsertAsync(plan, autoSave: true);

        await planRepository.InsertGatewayPlanAsync(
            new GatewayPlan(plan.Id, gateway, externalId));

        plan = await planRepository.GetAsync(id, includeDetails: true);

        plan.Id.ShouldBe(id);
        plan.GatewayPlans.ShouldNotBeEmpty();
        plan.GatewayPlans.Count.ShouldBe(1);

        var gatewayPlan = plan.GatewayPlans.First();
        gatewayPlan.PlanId.ShouldBe(plan.Id);
        gatewayPlan.ExternalId.ShouldBe(externalId);
    }

    [Fact]
    public async Task InsertGatewayPlanAsync_ShouldWorkProperly_WithExistingPlan()
    {
        var gateway = "YetAnotherGateway";

        await planRepository.InsertGatewayPlanAsync(
            new GatewayPlan(testData.Plan_1_Id, gateway, "some_secret_price_id"));

        var plan = await planRepository.GetAsync(testData.Plan_1_Id, includeDetails: true);

        plan.GatewayPlans.Count.ShouldBe(2);
        plan.GatewayPlans.ShouldContain(x => x.Gateway == gateway);
    }

    [Fact]
    public async Task GetGatewayPlanPagedListAsync_ShouldPaginationWork()
    {
        var maxResultCount = 3;
        var items = await planRepository.GetGatewayPlanPagedListAsync(testData.Plan_2_Id, 0, maxResultCount, null);

        items.ShouldNotBeEmpty();
        items.Count.ShouldBe(maxResultCount);
    }

    [Fact]
    public async Task GetGatewayPlanCountAsync_ShouldWorkProperly()
    {
        var count = await planRepository.GetGatewayPlanCountAsync(testData.Plan_2_Id);

        count.ShouldNotBe(0);
        count.ShouldBe(7);
    }

    [Fact]
    public async Task GetManyAsync_ShouldWorkProperly()
    {
        var ids = new[] { testData.Plan_1_Id, testData.Plan_2_Id };

        var plans = await planRepository.GetManyAsync(ids);

        plans.ShouldNotBeNull();
        plans.ShouldNotBeEmpty();
        plans.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetGatewayPlanPagedListAsync_ShouldReturnOneResult_WithGatewayFilter()
    {
        var filterKeyword = testData.GatewayPlan_1_ExternalId;
        var result = await planRepository.GetGatewayPlanPagedListAsync(testData.Plan_2_Id, 0, 32, null, filterKeyword);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }
    [Fact]
    public async Task GetGatewayPlanCountAsync_ShouldReturnOne_WithGatewayFilter()
    {
        var filterKeyword = "c";
        var result = await planRepository.GetFilteredCountAsync(filterKeyword);

        result.ShouldBe(1);
    }

    [Fact]
    public async Task GetPagedAndFilteredListAsync_ShouldWorkProperly_WithoutFilter()
    {
        var result = await planRepository.GetPagedAndFilteredListAsync(0, 32, null, null);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetPagedAndFilteredListAsync_ShouldReturnOneResult_WithFilter()
    {
        var filterKeyword = testData.Plan_1_Name.Substring(0, 5);
        var result = await planRepository.GetPagedAndFilteredListAsync(0, 32, null, filterKeyword);

        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }

    [Fact]
    public async Task GetFilteredCountAsync_ShouldReturnTwo_WithoutFilter()
    {
        var result = await planRepository.GetFilteredCountAsync(filter: null);

        result.ShouldBe(2);
    }

    [Fact]
    public async Task GetFilteredCountAsync_ShouldReturnOne_WithFilter()
    {
        var filterKeyword = testData.Plan_1_Name.Substring(0, 5);
        var result = await planRepository.GetFilteredCountAsync(filterKeyword);

        result.ShouldBe(1);
    }
}
