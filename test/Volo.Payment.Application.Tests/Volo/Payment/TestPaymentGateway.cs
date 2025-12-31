using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;

namespace Volo.Payment;

public class TestPaymentGateway : IPaymentGateway, ITransientDependency
{
    private readonly PaymentTestData testData;
    private readonly IPaymentRequestRepository paymentRequestRepository;

    public TestPaymentGateway(PaymentTestData testData, IPaymentRequestRepository paymentRequestRepository)
    {
        this.testData = testData;
        this.paymentRequestRepository = paymentRequestRepository;
    }

    public async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        var paymentRequest = await paymentRequestRepository.GetAsync(testData.PaymentRequest_3_Id);

        paymentRequest.Complete();

        await paymentRequestRepository.UpdateAsync(paymentRequest);

        return paymentRequest;
    }

    public Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        return Task.CompletedTask;
    }

    public bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        return true;
    }

    public Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        return Task.FromResult(new PaymentRequestStartResult
        {
            CheckoutLink = input.ReturnUrl
        });
    }
}
