using System.Threading.Tasks;

namespace Volo.CmsKit.PageFeedbacks;

public interface IPageFeedbackEntityTypeDefinitionStore : IEntityTypeDefinitionStore<PageFeedbackEntityTypeDefinition>
{
    Task<PageFeedbackEntityTypeDefinitions> GetPageFeedbackEntityTypeDefinitionsAsync();
}