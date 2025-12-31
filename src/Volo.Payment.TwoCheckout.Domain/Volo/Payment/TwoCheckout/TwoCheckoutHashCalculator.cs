using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Payment.TwoCheckout;

public class TwoCheckoutHashCalculator : ITransientDependency
{
    protected TwoCheckoutOptions TwoCheckoutOptions { get; }

    public TwoCheckoutHashCalculator(IOptions<TwoCheckoutOptions> options)
    {
        TwoCheckoutOptions = options.Value;
    }

    public virtual string GetMd5Hash(string hashString)
    {
        return HmacMd5HashHelper.GetMd5Hash(hashString, TwoCheckoutOptions.Signature);
    }

    public virtual string GetMd5HashForQueryStringParameters(string queryStringParams)
    {
        return GetMd5Hash(queryStringParams.Length + queryStringParams);
    }

    public virtual string GetHmacSha256Hash(string hashString)
    {
        return HmacSha256HashHelper.GetHmacSha256Hash(hashString, TwoCheckoutOptions.Signature);
    }

    public virtual string GetHmacSha256HashForQueryStringParameters(string queryStringParams)
    {
        return GetHmacSha256Hash(queryStringParams.Length + queryStringParams);
    }
}
