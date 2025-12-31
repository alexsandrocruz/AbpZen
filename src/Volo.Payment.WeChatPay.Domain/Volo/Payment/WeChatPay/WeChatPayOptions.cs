namespace Volo.Payment.WeChatPay;

public class WeChatPayOptions
{
    public string BaseUrl = "https://api.mch.weixin.qq.com/v3/pay";
    
    public string AppId { get; set; }
    
    public string MchId { get; set; }
    
    public string SerialNo { get; set; }
    
    public string PrivateKey { get; set; }
}