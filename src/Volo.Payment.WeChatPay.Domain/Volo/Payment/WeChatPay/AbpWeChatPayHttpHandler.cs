using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Volo.Payment.WeChatPay;

public class AbpWeChatPayHttpHandler : DelegatingHandler
{
    protected WeChatPayOptions WeChatPayOptions { get; }
 
    public AbpWeChatPayHttpHandler(IOptions<WeChatPayOptions> weChatPayOptions)
    {
        WeChatPayOptions = weChatPayOptions.Value;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await AuthorizationAsync(request);
        return await base.SendAsync(request, cancellationToken);
    }

    protected virtual async Task AuthorizationAsync(HttpRequestMessage request)
    {
        var method = request.Method.ToString();
        var body = "";
        if (method == "POST" || method == "PUT" || method == "PATCH")
        {
            var content = request.Content;
            body = await content.ReadAsStringAsync();
        }

        var uri = request.RequestUri.PathAndQuery;
        var timestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
        var nonce = Path.GetRandomFileName();

        var message = $"{method}\n{uri}\n{timestamp}\n{nonce}\n{body}\n";
        var signature = Sign(message);
        var auth = $"mchid=\"{WeChatPayOptions.MchId}\",nonce_str=\"{nonce}\",timestamp=\"{timestamp}\",serial_no=\"{WeChatPayOptions.SerialNo}\",signature=\"{signature}\"";
        
        request.Headers.Add("Authorization", $"WECHATPAY2-SHA256-RSA2048 {auth}");
    }
    
    protected virtual string Sign(string message)
    {
        var keyData = Convert.FromBase64String(WeChatPayOptions.PrivateKey);
        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(keyData, out _);
        var data = System.Text.Encoding.UTF8.GetBytes(message);
        return Convert.ToBase64String(rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1));
    }
}