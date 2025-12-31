using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Alipay.EasySDK.Factory;
using Alipay.EasySDK.Payment.Common.Models;
using Volo.Abp.DependencyInjection;
using Volo.Payment.Gateways;
using Volo.Payment.Requests;

namespace Volo.Payment.Alipay;

public class AlipayPaymentGateway : IPaymentGateway, ITransientDependency
{
    protected IPurchaseParameterListGenerator PurchaseParameterListGenerator { get; }
    protected IPaymentRequestRepository PaymentRequestRepository { get; }

    public AlipayPaymentGateway(IPurchaseParameterListGenerator purchaseParameterListGenerator, IPaymentRequestRepository paymentRequestRepository)
    {
        PurchaseParameterListGenerator = purchaseParameterListGenerator;
        PaymentRequestRepository = paymentRequestRepository;
    }

    public virtual Task<PaymentRequestStartResult> StartAsync(PaymentRequest paymentRequest, PaymentRequestStartInput input)
    {
        var purchaseParameters = PurchaseParameterListGenerator.GetExtraParameterConfiguration(paymentRequest);

        var goodsDetailList = new List<object>();
        foreach (var product in paymentRequest.Products)
        {
            var goodsDetail = new Dictionary<string, object>
            {
                { "goods_id", product.Code},
                { "goods_name", product.Name },
                { "quantity", product.Count },
                { "price", Convert.ToDecimal(product.UnitPrice).ToString("0.00", CultureInfo.InvariantCulture)}
            };
            goodsDetailList.Add(goodsDetail);
        }

        var result = Factory.Payment
            .Page()
            .Optional("goods_detail", goodsDetailList)
            .AsyncNotify(input.ReturnUrl)
            .Pay(
                string.Join(" ", paymentRequest.Products.Select(x=>x.Name)),
                paymentRequest.Id.ToString("N"),
                (Convert.ToDecimal(paymentRequest.Products.Sum(x => x.TotalPrice))).ToString("0.00", CultureInfo.InvariantCulture),
                input.ReturnUrl);

        return Task.FromResult(new PaymentRequestStartResult { CheckoutLink = result.Body });
    }

    public virtual async Task<PaymentRequest> CompleteAsync(Dictionary<string, string> parameters)
    {
        if (!parameters.TryGetValue("out_trade_no", out var tradeNo))
        {
            return null;
        }
        var order = await Factory.Payment.Common().QueryAsync(tradeNo);
        var paymentRequestId = Guid.Parse(order.OutTradeNo);
        var paymentRequest = await PaymentRequestRepository.GetAsync(paymentRequestId);

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
            paymentRequest.Failed(order.TradeStatus);
        }

        paymentRequest.Currency = order.PayCurrency;

        return await PaymentRequestRepository.UpdateAsync(paymentRequest);
    }

    public virtual Task HandleWebhookAsync(string payload, Dictionary<string, string> headers)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool IsValid(PaymentRequest paymentRequest, Dictionary<string, string> properties)
    {
        if (!properties.TryGetValue("trade_no", out var tradeNo))
        {
            return false;
        }
        var order =  Factory.Payment.Common().Query(tradeNo);
        return IsValid(paymentRequest, order);
    }

    protected virtual bool IsValid(PaymentRequest paymentRequest, AlipayTradeQueryResponse order)
    {
        if (paymentRequest.Id != Guid.Parse(order.OutTradeNo))
        {
            return false;
        }

        return order.TradeStatus == "TRADE_SUCCESS" && order.TotalAmount == Convert.ToDecimal(paymentRequest.Products.Sum(x => x.TotalPrice)).ToString("0.00", CultureInfo.InvariantCulture);
    }
}
