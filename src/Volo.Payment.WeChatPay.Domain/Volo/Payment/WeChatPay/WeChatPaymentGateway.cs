using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;

namespace Volo.Payment.WeChatPay;

public class WeChatPaymentGateway: IPaymentGateway, ITransientDependency
{
    protected IWeChatPayService WeChatPayService { get; }
    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }
    protected IPaymentRequestRepository PaymentRequestRepository { get; }
    
    public WeChatPaymentGateway(
        IWeChatPayService weChatPayService,
        IPurchaseParameterListGenerator purchaseParameterListGenerator,
        IPaymentRequestRepository paymentRequestRepository)
    {
        WeChatPayService = weChatPayService;
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        PaymentRequestRepository = paymentRequestRepository;
    }
    
    public virtual async Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var purchaseParameters = PurchaseParameterListGenerator.GetExtraParameterConfiguration(paymentRequest);
        
        var result = await WeChatPayService.PayH5Async(
            paymentRequest.Id.ToString("N"),
            string.Join(" ", paymentRequest.Products.Select(x => x.Name)),
            paymentRequest.Products.Sum(x => x.TotalPrice),
            "CNY", //WeChat pay only support CNY currency,
            input.ReturnUrl,
            input.GetProperty<string>(WeChatPaymentParameterConsts.PayerIp)
        );

        return new PaymentRequestStartResult { CheckoutLink = result };
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        if (!parameters.TryGetValue("out_trade_no", out var outTradeNo))
        {
            return null;
        }
        
        var order = await WeChatPayService.QueryAsync(outTradeNo);
        var paymentRequest = await PaymentRequestRepository.FindAsync(Guid.Parse(order.OutTradeNo));
        
        if (paymentRequest.State == PaymentRequestState.Completed)
        {
            return paymentRequest;
        }

        if (IsValid(paymentRequest, order))
        {
            paymentRequest.Complete();
        }
        else
        {
            paymentRequest.Failed($"{order.TradeState}: {order.TradeStateDesc}");
        }

        paymentRequest.Currency = order.Currency;
        
        return await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    public virtual Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        throw new System.NotImplementedException();
    }
    
    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        if (!properties.TryGetValue("trade_no", out var outTradeNo))
        {
            return false;
        }

        var order = AsyncHelper.RunSync(() => WeChatPayService.QueryAsync(outTradeNo));
        return IsValid(paymentRequest, order);
    }

    private bool IsValid(PaymentRequest paymentRequest, PayResultResponse order)
    {
        if (paymentRequest.Id != Guid.Parse(order.OutTradeNo))
        {
            return false;
        }
        
        return order.IsSuccess() && order.Total == (int)paymentRequest.Products.Sum(x => x.TotalPrice) * 100;
    }
}