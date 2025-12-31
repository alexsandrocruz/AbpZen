using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Payment.Requests;

public interface IPaymentRequestAppService : IApplicationService
{
    Task<PaymentRequestWithDetailsDto> CreateAsync(PaymentRequestCreateDto input);

    Task<PaymentRequestWithDetailsDto> GetAsync(Guid id);

    Task<PaymentRequestStartResultDto> StartAsync(string paymentGateway, PaymentRequestStartDto input);

    Task<PaymentRequestWithDetailsDto> CompleteAsync(string paymentGateway, Dictionary<string, string> parameters);

    Task<bool> HandleWebhookAsync(string paymentGateway, string payload, Dictionary<string,string> headers);
}
