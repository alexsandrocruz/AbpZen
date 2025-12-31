namespace Volo.Payment.WeChatPay;

public class WeChatPayConsts
{
    /// <summary>
    /// Value: "wechat"
    /// </summary>
    public const string GatewayName = "WeChatPay";
    
    /// <summary>
    /// Value: "/Payment/WeChat/PrePayment"
    /// </summary>
    public const string PrePaymentUrl = "/Payment/WeChatPay/PrePayment";

    /// <summary>
    /// Value: "/Payment/WeChat/PostPayment"
    /// </summary>
    public const string PostPaymentUrl = "/Payment/WeChatPay/PostPayment";
}