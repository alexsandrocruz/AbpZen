using Volo.Payment.Requests;

namespace Volo.Payment.PayPal;

public interface IPurchaseParameterListGenerator
{
    PayPalPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(
        PaymentRequest paymentRequest);
}
