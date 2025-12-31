namespace Volo.Payment.Payu;

public static class PayuConsts
{
    /// <summary>
    /// Value: "payu"
    /// </summary>
    public const string GatewayName = "payu";

    /// <summary>
    /// Value: "/Payment/Payu/PrePayment"
    /// </summary>
    public const string PrePaymentUrl = "/Payment/Payu/PrePayment";

    /// <summary>
    /// Value: "/Payment/Payu/PostPayment"
    /// </summary>
    public const string PostPaymentUrl = "/Payment/Payu/PostPayment";

    public static class ParameterNames
    {
        public const string PaymentRequestId = "PaymentRequestId";
        public const string Url = "url";
        public const string PayRefNo = "payrefno";
        public const string Ctrl = "ctrl";
    }
}
