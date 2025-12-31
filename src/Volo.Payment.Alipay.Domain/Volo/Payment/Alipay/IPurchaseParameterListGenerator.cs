using Volo.Payment.Requests;

namespace Volo.Payment.Alipay;

public interface IPurchaseParameterListGenerator
{
    AlipayPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest);
}
