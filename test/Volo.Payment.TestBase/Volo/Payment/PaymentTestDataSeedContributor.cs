using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Plans;
using Volo.Payment.Requests;

namespace Volo.Payment.TestBase.Volo.Payment;

public class PaymentTestDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly PaymentTestData testData;
    private readonly IPlanRepository planRepository;
    private readonly IPaymentRequestRepository paymentRequestRepository;

    public PaymentTestDataSeedContributor(
        PaymentTestData testData,
        IPlanRepository planRepository,
        IPaymentRequestRepository paymentRequestRepository)
    {
        this.testData = testData;
        this.planRepository = planRepository;
        this.paymentRequestRepository = paymentRequestRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        await SeedPlansAsync();
        await SeedPaymentRequests();
    }

    private async Task SeedPlansAsync()
    {
        var plan1 = new Plan(testData.Plan_1_Id, testData.Plan_1_Name);

        plan1.GatewayPlans.Add(
            new GatewayPlan(
                testData.Plan_1_Id,
                testData.GatewayPlan_1_Gateway,
                testData.GatewayPlan_1_ExternalId));

        await planRepository.InsertAsync(plan1);

        var plan2 = new Plan(testData.Plan_2_Id, testData.Plan_2_Name);
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway A", testData.GatewayPlan_1_ExternalId));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway B", "prc_4444_1111"));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway C", "prc_3333_4444"));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway D", "prc_5555_6655"));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway E", "prc_5555_6655"));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway F", "prc_5556_6657"));
        plan2.GatewayPlans.Add(new GatewayPlan(testData.Plan_2_Id, "Gateway G", "prc_5558_6659"));

        await planRepository.InsertAsync(plan2);
    }

    private async Task SeedPaymentRequests()
    {
        var paymentRequest1 = new PaymentRequest(testData.PaymentRequest_1_Id, testData.PaymentRequest_1_Currency)
        {
            Gateway = testData.PaymentRequest_1_Gateway
        };
        paymentRequest1.SetExternalSubscriptionId(testData.PaymentRequest_1_SubscriptionId);
        paymentRequest1.Complete();

        var paymentRequest2 = new PaymentRequest(testData.PaymentRequest_2_Id, testData.PaymentRequest_2_Currency)
        {
            Gateway = testData.PaymentRequest_2_Gateway
        };
        paymentRequest2.SetExternalSubscriptionId(testData.PaymentRequest_2_SubscriptionId);

        var paymentRequest3 = new PaymentRequest(testData.PaymentRequest_3_Id, testData.PaymentRequest_3_Currency)
        {
            Gateway = testData.PaymentRequest_3_Gateway
        };

        await paymentRequestRepository.InsertManyAsync(new[]
        {
                paymentRequest1,
                paymentRequest2,
                paymentRequest3
        });
    }
}
