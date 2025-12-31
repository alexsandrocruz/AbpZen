using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.OpenIddict;

namespace Volo.Abp.Account.Web.Pages.Account;

public class OpenIddictReturnUrlRequestCultureProvider: RequestCultureProvider
{
    public readonly string ReturnUrl = "ReturnUrl";

    public readonly string QueryStringKey = "culture";

    public readonly string UIQueryStringKey = "ui-culture";

    public async override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        var request = httpContext.Request;
        if (!request.QueryString.HasValue)
        {
            return await NullProviderCultureResult;
        }

        string returnUrl = request.Query[ReturnUrl];
        if (returnUrl.IsNullOrWhiteSpace())
        {
            return await NullProviderCultureResult;
        }

        var openIddictRequestHelper = httpContext.RequestServices.GetService<AbpOpenIddictRequestHelper>();
        if (openIddictRequestHelper == null)
        {
            return await NullProviderCultureResult;
        }

        var openIddictRequest = await openIddictRequestHelper.GetFromReturnUrlAsync(returnUrl);
        if (openIddictRequest == null)
        {
            return await NullProviderCultureResult;
        }

        var queryCulture = openIddictRequest.GetParameter(QueryStringKey).ToString();
        var queryUICulture = openIddictRequest.GetParameter(UIQueryStringKey).ToString();

        if (queryCulture == null && queryUICulture == null)
        {
            // No values specified for either so no match
            return await NullProviderCultureResult;
        }

        if (queryCulture != null && queryUICulture == null)
        {
            // Value for culture but not for UI culture so default to culture value for both
            queryUICulture = queryCulture;
        }
        else if (queryCulture == null && queryUICulture != null)
        {
            // Value for UI culture but not for culture so default to UI culture value for both
            queryCulture = queryUICulture;
        }

        return new ProviderCultureResult(queryCulture, queryUICulture);
    }
}
