using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Volo.Abp.Options;
using Volo.Abp.UI.Navigation.Urls;

namespace Volo.Payment;

public class PaymentWebOptionsManager : AbpDynamicOptionsManager<PaymentWebOptions>
{
    protected IAppUrlProvider UrlProvider { get; }
    
    public PaymentWebOptionsManager(
        [NotNull] IOptionsFactory<PaymentWebOptions> factory, IAppUrlProvider urlProvider) : base(factory)
    {
        UrlProvider = urlProvider;
    }

    protected async override Task OverrideOptionsAsync(string name, PaymentWebOptions options)
    {
        options.RootUrl = await UrlProvider.NormalizeUrlAsync(options.RootUrl);
        options.CallbackUrl = await UrlProvider.NormalizeUrlAsync(options.CallbackUrl);
        
        foreach (var gateway in options.Gateways)
        {
            gateway.Value.PostPaymentUrl = await UrlProvider.NormalizeUrlAsync(gateway.Value.PostPaymentUrl);
            gateway.Value.PrePaymentUrl = await UrlProvider.NormalizeUrlAsync(gateway.Value.PrePaymentUrl);
        }
    }
}