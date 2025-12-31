using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.EventBus.Distributed;
using Volo.Payment.Gateways;

namespace Volo.Payment.Requests;

public class PaymentRequestAppService : PaymentAppServiceBase, IPaymentRequestAppService
{
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    protected PaymentGatewayResolver PaymentGatewayResolver { get; }

    protected IDistributedEventBus DistributedEventBus { get; }

    public PaymentRequestAppService(
        IPaymentRequestRepository paymentRequestRepository,
        PaymentGatewayResolver paymentMethodResolver,
        IDistributedEventBus distributedEventBus)
    {
        PaymentRequestRepository = paymentRequestRepository;
        PaymentGatewayResolver = paymentMethodResolver;
        DistributedEventBus = distributedEventBus;
    }

    public virtual async Task<PaymentRequestWithDetailsDto> CreateAsync(PaymentRequestCreateDto input)
    {
        var paymentRequest = new PaymentRequest(GuidGenerator.Create(), input.Currency);

        foreach (var extraProperty in input.ExtraProperties)
        {
            paymentRequest.SetProperty(extraProperty.Key, extraProperty.Value);
        }

        foreach (var productDto in input.Products)
        {
            paymentRequest.AddProduct(
                productDto.Code,
                productDto.Name,
                productDto.PaymentType,
                productDto.UnitPrice,
                productDto.Count,
                productDto.PlanId,
                productDto.TotalPrice,
                productDto.ExtraProperties);
        }

        var insertedPaymentRequest = await PaymentRequestRepository.InsertAsync(paymentRequest, autoSave: true);

        return ObjectMapper.Map<PaymentRequest, PaymentRequestWithDetailsDto>(insertedPaymentRequest);
    }

    public virtual async Task<PaymentRequestWithDetailsDto> GetAsync(Guid id)
    {
        var paymentRequest = await PaymentRequestRepository.GetAsync(id);
        return await GetPaymentRequestWithDetailsDtoAsync(paymentRequest);
    }

    public virtual async Task<PaymentRequestStartResultDto> StartAsync(string gateway, PaymentRequestStartDto inputDto)
    {
        var paymentRequest = await PaymentRequestRepository.GetAsync(inputDto.PaymentRequestId, includeDetails: true);
        paymentRequest.Gateway = gateway;
        await PaymentRequestRepository.UpdateAsync(paymentRequest);
        

        var startInput = ObjectMapper.Map<PaymentRequestStartDto, PaymentRequestStartInput>(inputDto);
        MapExtraProperties(inputDto, startInput); // TODO: Find out why ExtraProperties mapping doesn't work?
        var result = await PaymentGatewayResolver.Resolve(gateway).StartAsync(paymentRequest, startInput);

        var resultDto = ObjectMapper.Map<PaymentRequestStartResult, PaymentRequestStartResultDto>(result);
        MapExtraProperties(result, resultDto); // TODO: Find out why ExtraProperties mapping doesn't work?

        return resultDto;
    }

    public virtual async Task<PaymentRequestWithDetailsDto> CompleteAsync(string paymentGateway, Dictionary<string, string> parameters)
    {
        var paymentRequest = await PaymentGatewayResolver.Resolve(paymentGateway).CompleteAsync(parameters);

        if (paymentRequest.State == PaymentRequestState.Completed)
        {
            await DistributedEventBus.PublishAsync(
                new PaymentRequestCompletedEto(
                    id: paymentRequest.Id,
                    gateway: paymentRequest.Gateway,
                    currency: paymentRequest.Currency,
                    products: ObjectMapper.Map<ICollection<PaymentRequestProduct>, List<PaymentRequestProductCompletedEto>>(paymentRequest.Products),
                    extraProperties: paymentRequest.ExtraProperties));
        }

        return await GetPaymentRequestWithDetailsDtoAsync(paymentRequest);
    }

    public virtual async Task<bool> HandleWebhookAsync(string paymentGateway, string payload, Dictionary<string, string> headers)
    {
        await PaymentGatewayResolver.Resolve(paymentGateway).HandleWebhookAsync(payload, headers);
        return true;
    }

    private async Task<PaymentRequestWithDetailsDto> GetPaymentRequestWithDetailsDtoAsync(
     PaymentRequest paymentRequest)
    {
        return ObjectMapper.Map<PaymentRequest, PaymentRequestWithDetailsDto>(
            await PaymentRequestRepository.GetAsync(paymentRequest.Id, includeDetails: true)
        );
    }

    private void MapExtraProperties(IHasExtraProperties source, IHasExtraProperties destination)
    {
        foreach (var key in source.ExtraProperties.Keys)
        {
            destination.ExtraProperties[key] = source.ExtraProperties[key];
        }
    }
}
