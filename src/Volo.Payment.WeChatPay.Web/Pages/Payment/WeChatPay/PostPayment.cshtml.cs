using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Volo.Abp.Json;
using Volo.Payment.Requests;

namespace Volo.Payment.WeChatPay.Pages.Payment.WeChatPay;

[IgnoreAntiforgeryToken]
public class PostPaymentModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string Token { get; set; }

    protected IPaymentRequestAppService PaymentRequestAppService { get; }
    
    protected IJsonSerializer JsonSerializer { get; }
    
    protected WeChatPayWebOptions WebOptions { get; }

    public ILogger<PostPaymentModel> Logger { get; set; }

    private readonly IOptions<PaymentWebOptions> _paymentWebOptions;

    public PostPaymentModel(
        IPaymentRequestAppService paymentRequestAppService,
        IOptions<PaymentWebOptions> paymentWebOptions,
        IOptions<WeChatPayWebOptions> weChatPayWebOptions,
        IJsonSerializer jsonSerializer)
    {
        PaymentRequestAppService = paymentRequestAppService;
        _paymentWebOptions = paymentWebOptions;
        WebOptions = weChatPayWebOptions.Value;
        JsonSerializer = jsonSerializer;
        Logger = NullLogger<PostPaymentModel>.Instance;
    }

    public virtual IActionResult OnGet()
    {
        return Page();
    }


    public virtual async Task<IActionResult> OnPostAsync(IFormCollection formCollection)
    {
        await _paymentWebOptions.SetAsync();
        
        var memoryStream = new MemoryStream();
        await Response.Body.CopyToAsync(memoryStream);

        var content = Encoding.UTF8.GetString(memoryStream.GetBuffer());
        var notifyModel = JsonSerializer.Deserialize<WeChatPayNotifyModel>(content);
        
        var decryptStr = AesGcm.AesGcmDecrypt(notifyModel.resource.associated_data, notifyModel.resource.nonce, notifyModel.resource.ciphertext, WebOptions.AesKey);
        var decryptModel = JsonSerializer.Deserialize<WeChatPayResourceDecryptModel>(decryptStr);
        
        var paymentRequest = await PaymentRequestAppService.CompleteAsync(WeChatPayConsts.GatewayName, new Dictionary<string, string>{{"out_trade_no", decryptModel.out_trade_no}});

        if (paymentRequest == null)
        {
            return BadRequest();
        }

        return new OkResult();
    }
}
