using Volo.Payment.Requests;

namespace Volo.Payment.Iyzico;

public interface IPurchaseParameterListGenerator
{
    IyzicoPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest);
}
