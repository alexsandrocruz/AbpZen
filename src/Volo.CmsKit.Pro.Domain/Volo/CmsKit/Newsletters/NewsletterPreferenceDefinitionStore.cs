using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Newsletters;

public class NewsletterPreferenceDefinitionStore : INewsletterPreferenceDefinitionStore, ITransientDependency
{
    protected NewsletterOptions NewsletterOptions { get; }

    public NewsletterPreferenceDefinitionStore(IOptions<NewsletterOptions> newsletterOptions)
    {
        NewsletterOptions = newsletterOptions.Value;
    }

    public Task<List<NewsletterPreferenceDefinition>> GetNewslettersAsync()
    {
        return Task.FromResult(NewsletterOptions.Preferences.Values.ToList());
    }
}
