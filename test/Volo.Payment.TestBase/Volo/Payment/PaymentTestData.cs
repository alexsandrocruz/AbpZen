using System;
using Volo.Abp.DependencyInjection;

namespace Volo.Payment;

public class PaymentTestData : ISingletonDependency
{
    public Guid Plan_1_Id { get; } = Guid.NewGuid();
    public string Plan_1_Name { get; } = "Basic Plan";
    public string GatewayPlan_1_Gateway { get; } = "Stripe";
    public string GatewayPlan_1_ExternalId { get; } = "price_232ds456534";

    public Guid Plan_2_Id { get; } = Guid.NewGuid();
    public string Plan_2_Name { get; } = "Another Plan";
    public Guid PaymentRequest_1_Id { get; } = Guid.NewGuid();
    public string PaymentRequest_1_SubscriptionId { get; } = "sub_123456789_some_gateway_data_id";
    public string PaymentRequest_1_Gateway { get; } = "Stripe";
    public string PaymentRequest_1_Currency { get; } = "USD";

    public Guid PaymentRequest_2_Id { get; } = Guid.NewGuid();
    public string PaymentRequest_2_SubscriptionId { get; } = "sub_987654321_some_gateway_data_id";
    public string PaymentRequest_2_Gateway { get; } = "Stripe";
    public string PaymentRequest_2_Currency { get; } = "USD";

    public Guid PaymentRequest_3_Id { get; } = Guid.NewGuid();
    public string PaymentRequest_3_Gateway { get; } = "Test";
    public string PaymentRequest_3_Currency { get; } = "USD";
}
