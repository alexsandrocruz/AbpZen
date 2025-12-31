namespace Volo.Payment.Stripe;

public static class StripeConsts
{
    /// <summary>
    /// Value: "stripe"
    /// </summary>
    public const string GatewayName = "stripe";

    /// <summary>
    /// Value: "/Payment/Stripe/PrePayment"
    /// </summary>
    public const string PrePaymentUrl = "/Payment/Stripe/PrePayment";

    /// <summary>
    /// Value: "/Payment/Stripe/PostPayment"
    /// </summary>
    public const string PostPaymentUrl = "/Payment/Stripe/PostPayment";

    public static class ParameterNames
    {
        public const string PaymentRequestId = "PaymentRequestId";
        public const string PublishableKey = "PublishableKey";
        public const string SessionId = "SessionId";
    }
}
