using Volo.Payment.Requests;

namespace Volo.Payment.Stripe;

public interface IPurchaseParameterListGenerator
{
    StripePaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest);
}
