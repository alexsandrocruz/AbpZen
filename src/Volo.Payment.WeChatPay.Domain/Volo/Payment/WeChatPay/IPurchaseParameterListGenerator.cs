using Volo.Payment.Requests;

namespace Volo.Payment.WeChatPay;

public interface IPurchaseParameterListGenerator
{
    WeChatPayRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest);
}