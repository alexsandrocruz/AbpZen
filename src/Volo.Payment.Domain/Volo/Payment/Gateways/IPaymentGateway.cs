using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Payment.Requests;

namespace Volo.Payment.Gateways;

public interface IPaymentGateway
{
    bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties);

    public Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input);

    public Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters);

    public Task HandleWebhookAsync(string payload, Dictionary<string, string> headers);
}
