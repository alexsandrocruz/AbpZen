using IdentityModel.OidcClient.Browser;
using Volo.Abp.DependencyInjection;
using IBrowser = IdentityModel.OidcClient.Browser.IBrowser;

namespace LeptonXDemoApp.MauiBlazor.OAuth;

public class MauiAuthenticationBrowser : IBrowser, ITransientDependency
{
    public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
    {
        try
        {
            WebAuthenticatorResult result = null;
            var webAuthenticatorOptions = new WebAuthenticatorOptions
            {
                Url = new Uri(options.StartUrl),
                CallbackUrl = new Uri(options.EndUrl),
                PrefersEphemeralWebBrowserSession = true
            };


#if WINDOWS
            result =
 await Platforms.Windows.WebAuthenticator.AuthenticateAsync(webAuthenticatorOptions.Url, webAuthenticatorOptions.CallbackUrl);
#else
            result = await WebAuthenticator.AuthenticateAsync(new WebAuthenticatorOptions
            {
                Url = new Uri(options.StartUrl),
                CallbackUrl = new Uri(options.EndUrl),
                PrefersEphemeralWebBrowserSession = true
            });
#endif


            return new BrowserResult
            {
                Response = ToRawIdentityUrl(options.EndUrl, result)
            };
        }
        catch (TaskCanceledException)
        {
            return new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel
            };
        }
        catch (Exception ex)
        {
            return new BrowserResult
            {
                ResultType = BrowserResultType.UnknownError,
                Error = ex.ToString()
            };
        }
    }

    private static string ToRawIdentityUrl(string redirectUrl, WebAuthenticatorResult result)
    {
        if (DeviceInfo.Platform == DevicePlatform.WinUI)
        {
            var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
            var modifiedParameters = parameters.ToList();

            var stateParameter = modifiedParameters
                .FirstOrDefault(p => p.StartsWith("state", StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(stateParameter))
            {
                // Remove the state key added by WebAuthenticator that includes appInstanceId
                modifiedParameters = modifiedParameters
                    .Where(p => !p.StartsWith("state", StringComparison.OrdinalIgnoreCase)).ToList();

                stateParameter = System.Web.HttpUtility.UrlDecode(stateParameter).Split('&').Last();
                modifiedParameters.Add(stateParameter);
            }

            var values = string.Join("&", modifiedParameters);
            return $"{redirectUrl}#{values}";
        }
        else
        {
            var parameters = result.Properties.Select(pair => $"{pair.Key}={pair.Value}");
            var values = string.Join("&", parameters);

            return $"{redirectUrl}#{values}";
        }
    }
}
