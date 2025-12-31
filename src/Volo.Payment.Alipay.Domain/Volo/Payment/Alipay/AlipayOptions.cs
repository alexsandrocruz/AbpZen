namespace Volo.Payment.Alipay;

public class AlipayOptions
{
    public string Protocol { get; set; }

    public string GatewayHost { get; set; } 
    
    public string SignType { get; set; }
    
    public string AppId { get; set; }
    
    public string MerchantPrivateKey { get; set; }
    
    public string MerchantCertPath { get; set; }
    
    public string AlipayCertPath { get; set; }
    
    public string AlipayRootCertPath { get; set; }
    
    public string AlipayPublicKey { get; set; }
    
    public string NotifyUrl { get; set; }
    
    public string EncryptKey { get; set; }
    
}