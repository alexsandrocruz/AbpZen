using System.Threading.Tasks;

namespace Volo.Payment.WeChatPay;

public interface IWeChatPayService
{
    public const string HttpClientName = "PaymentWeChatPayClient";

    Task<string> PayH5Async(
        string outTradeNo,
        string description,
        float amount,
        string currency,
        string notifyUrl,
        string payerIp);

    Task<PayResultResponse> QueryAsync(string outTradeNo);
}