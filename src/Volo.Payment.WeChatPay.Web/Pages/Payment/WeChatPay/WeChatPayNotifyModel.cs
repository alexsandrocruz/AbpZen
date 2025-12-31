namespace Volo.Payment.WeChatPay.Pages.Payment.WeChatPay;

public class WeChatPayNotifyModel
{
    public string id { set; get; }
    
    public string create_time { set; get; }
    
    public string event_type { set; get; }
    
    public string resource_type { set; get; }
    
    public WeChatPayResourceModel resource { set; get; }
    
    public string summary { set; get; }

}

public class WeChatPayResourceModel
{
    public string algorithm { set; get; }
 
    public string ciphertext { set; get; }
 
    public string associated_data { set; get; }
 
    public string original_type { set; get; }
 
    public string nonce { set; get; }
}

public class WeChatPayResourceDecryptModel
{
    public string appid { set; get; }
    
    public string mchid { set; get; }
    
    public string out_trade_no { set; get; }
    
    public string transaction_id { set; get; }
}