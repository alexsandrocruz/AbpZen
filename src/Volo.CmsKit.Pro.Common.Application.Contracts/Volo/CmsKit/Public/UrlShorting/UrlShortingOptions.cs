using System;
using System.Linq;
using System.Threading.Tasks;

namespace Volo.CmsKit.Public.UrlShorting;

public class UrlShortingOptions
{
    public Func<ConflictUrlContext, Task<ShortenedUrlDto>> OnConflict { get; set; } = context => Task.FromResult(context.ConflictUrls.FirstOrDefault());
    public bool RegexIgnoreCase { get; set; } = true;

    /// <summary>
    /// Used when <see cref="UrlShortingOptions.PreventRegexLoop"/> is true. Default value: "__redirect"
    /// </summary>
    public string TrackingQueryStringParameter { get; set; } = "__redirect";

    /// <summary>
    /// Prevents executing the same regex rule multiple times by using a query string parameter. (<see cref="TrackingQueryStringParameter"/>) Default value: true
    /// </summary>
    public bool PreventRegexLoop { get; set; } = true;
}

public class ConflictUrlContext
{
    public string SourceUrl { get; }
    public ShortenedUrlDto[] ConflictUrls { get; }

    public ConflictUrlContext(string sourceUrl, ShortenedUrlDto[] conflictUrls)
    {
        SourceUrl = sourceUrl;
        ConflictUrls = conflictUrls;
    }
}