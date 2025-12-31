using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Payment.WeChatPay;

public class WeChatPayService : IWeChatPayService, ITransientDependency
{
    protected IHttpClientFactory HttpClientFactory { get; }
    protected WeChatPayOptions Options { get; }
    
    protected IJsonSerializer JsonSerializer { get; }

    public WeChatPayService(IHttpClientFactory httpClientFactory, IOptions<WeChatPayOptions> options, IJsonSerializer jsonSerializer)
    {
        HttpClientFactory = httpClientFactory;
        JsonSerializer = jsonSerializer;
        Options = options.Value;
    }

    public virtual async Task<string> PayH5Async(
        string outTradeNo, 
        string description, 
        float amount,
        string currency,
        string notifyUrl,
        string payerIp)
        
    {
        var httpClient = HttpClientFactory.CreateClient(IWeChatPayService.HttpClientName);
        var url = "transactions/h5";
        
        var body = new
        {
            mchid = Options.MchId,
            appid = Options.AppId,
            out_trade_no = outTradeNo,
            description,
            amount = new {
                total = Convert.ToDecimal(amount) * 100,
                currency
            },
            notify_url = notifyUrl,
            scene_info = new {
                payer_client_ip = payerIp,
                h5_info = new {
                    type = "Wap"
                }
            }
        };
        
        var response = await httpClient.PostAsync(url, new StringContent(JsonSerializer.Serialize(body), System.Text.Encoding.UTF8, "application/json"));
        var result = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<dynamic>(result).h5_url;
    }

    public virtual async Task<PayResultResponse> QueryAsync(string outTradeNo)
    {
        var httpClient = HttpClientFactory.CreateClient(IWeChatPayService.HttpClientName);
        var url = $"transactions/out-trade-no/{outTradeNo}?mechid={Options.MchId}";

        var response = await httpClient.GetAsync(url);
        var result = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync());

        return new PayResultResponse
        {
            TradeState = result.trade_state,
            TradeStateDesc = result.trade_state_desc,
            Total = result.amount.total,
            Currency = result.amount.currency,
            OutTradeNo = result.out_trade_no,
        };
    }
}

public class PayResultResponse
{
    public string OutTradeNo { get; set; }
    
    public string TradeState { get; set; }
        
    public string TradeStateDesc { get; set; }
        
    public int Total { get; set; }
        
    public string Currency { get; set; }
        
    public bool IsSuccess()
    {
        return TradeState == "SUCCESS";
    }
}