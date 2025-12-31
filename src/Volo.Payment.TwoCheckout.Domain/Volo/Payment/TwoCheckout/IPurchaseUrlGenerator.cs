using Volo.Payment.Requests;

namespace Volo.Payment.TwoCheckout.Pages.Payment.TwoCheckout;

public interface IPurchaseUrlGenerator
{
    string GetUrl(PaymentRequest paymentRequest, string returnUrl);

    TwoCheckoutPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest, string returnUrl);
}
