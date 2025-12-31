using System.Collections.Generic;
using Volo.Payment.Requests;

namespace Volo.Payment.Payu.Pages.Payment.Payu;

public interface IPurchaseParameterListGenerator
{
    List<PurchaseParameter> Generate(PaymentRequest paymentRequest, string callbackUrl = null);

    PayuPaymentRequestExtraParameterConfiguration GetExtraParameterConfiguration(PaymentRequest paymentRequest);
}
