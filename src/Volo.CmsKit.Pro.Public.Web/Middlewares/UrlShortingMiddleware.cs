using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.CmsKit.Public.UrlShorting;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.GlobalFeatures;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using Volo.Abp.Features;
using Volo.CmsKit.Features;
using Volo.Abp.Caching;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.Pro.Public.Web.Middlewares;

public class UrlShortingMiddleware : IMiddleware, ITransientDependency
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var featureChecker = context.RequestServices.GetRequiredService<IFeatureChecker>();

        if (!GlobalFeatureManager.Instance.IsEnabled<UrlShortingFeature>() || !(await featureChecker.IsEnabledAsync(CmsKitProFeatures.UrlShortingEnable)))
        {
            await next(context);
            return;
        }
        
        context.Response.OnStarting(async ()  =>
        {
            if (context.Response.StatusCode != (int) HttpStatusCode.NotFound && !context.Items.ContainsKey("CmsKitResponseStatusCode"))
            {
                return;
            }

            var urlShortingPublicAppService = context.RequestServices.GetRequiredService<IUrlShortingPublicAppService>();
            var urlShortingCache = context.RequestServices.GetRequiredService<IDistributedCache<ShortenedUrlCacheItem, string>>();
            var sourceUrl = context.Request.Path + context.Request.QueryString;

            var shortenedUrl = await urlShortingCache.GetAsync(sourceUrl);

            if (shortenedUrl == null)
            {
                var shortenedUrlDto = await urlShortingPublicAppService.FindBySourceAsync(sourceUrl);

                if (shortenedUrlDto != null)
                {
                    shortenedUrl = new ShortenedUrlCacheItem
                    {
                        Exists = true,
                        IsRegex = shortenedUrlDto.IsRegex,
                        Id = shortenedUrlDto.Id,
                        Source = shortenedUrlDto.Source,
                        Target = shortenedUrlDto.Target
                    };
                }
            }

            if (shortenedUrl == null)
            {
                return;
            }

            if (shortenedUrl.IsRegex)
            {
                var options = context.RequestServices.GetRequiredService<IOptions<UrlShortingOptions>>().Value;
                var redirectionHash = shortenedUrl.Source.ToMd5();

                if (options.PreventRegexLoop)
                {
                    if (context.Request.Query.TryGetValue(options.TrackingQueryStringParameter, out var redirectedBy)
                        && redirectedBy.Contains(redirectionHash))
                    {
                        return;
                    }
                }
                    
                var targetUrl = options.RegexIgnoreCase
                    ? Regex.Replace(sourceUrl, shortenedUrl.Source, shortenedUrl.Target,
                        RegexOptions.IgnoreCase)
                    : Regex.Replace(sourceUrl, shortenedUrl.Source, shortenedUrl.Target);

                if (options.PreventRegexLoop && IsHostNameSame(context.Request, targetUrl))
                {
                    targetUrl = QueryHelpers.AddQueryString(targetUrl, new Dictionary<string, string>
                    {
                        { options.TrackingQueryStringParameter, redirectionHash }
                    });
                }

                context.Response.Redirect(targetUrl, true);
            }
            else
            {
                context.Response.Redirect(shortenedUrl.Target, true);
            }
        });

       
        try
        {
            await next(context);
        }
        finally
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                context.Items["CmsKitResponseStatusCode"] = context.Response.StatusCode;
            }
        }
    }

    private bool IsHostNameSame(HttpRequest httpRequest, string targetUrl)
    {
        var currentHostName = httpRequest.Host.Host;
        if (Uri.TryCreate(targetUrl, UriKind.Absolute, out var targetUri))
        {
            return currentHostName == targetUri.Host;
        }

        return true;
    }
}
