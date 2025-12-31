using System.Collections.Generic;
using System.Threading.Tasks;

namespace Volo.CmsKit.Newsletters;

public interface INewsletterPreferenceDefinitionStore
{
    Task<List<NewsletterPreferenceDefinition>> GetNewslettersAsync();
}
