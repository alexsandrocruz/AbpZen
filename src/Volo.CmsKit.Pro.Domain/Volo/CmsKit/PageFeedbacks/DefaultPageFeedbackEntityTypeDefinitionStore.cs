using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp;

namespace Volo.CmsKit.PageFeedbacks;

public class DefaultPageFeedbackEntityTypeDefinitionStore : IPageFeedbackEntityTypeDefinitionStore
{
    protected virtual CmsKitPageFeedbackOptions Options { get; }

    public DefaultPageFeedbackEntityTypeDefinitionStore(IOptions<CmsKitPageFeedbackOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<PageFeedbackEntityTypeDefinitions> GetPageFeedbackEntityTypeDefinitionsAsync()
    {
        return Task.FromResult(Options.EntityTypes);
    }

    public virtual Task<PageFeedbackEntityTypeDefinition> GetAsync(string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType), PageFeedbackConst.MaxEntityTypeLength);

        return Task.FromResult(
            Options.EntityTypes.SingleOrDefault(x => x.EntityType == entityType) ??
            throw new EntityCantHavePageFeedbackException(entityType)
        );
    }

    public virtual Task<bool> IsDefinedAsync(string entityType)
    {
        Check.NotNullOrWhiteSpace(entityType, nameof(entityType), PageFeedbackConst.MaxEntityTypeLength);

        return Task.FromResult(Options.EntityTypes.Any(x => x.EntityType == entityType));
    }
}