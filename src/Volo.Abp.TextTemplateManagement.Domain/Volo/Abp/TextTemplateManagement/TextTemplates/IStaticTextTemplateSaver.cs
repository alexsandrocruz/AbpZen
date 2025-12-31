using System.Threading.Tasks;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

public interface IStaticTextTemplateSaver
{
    Task SaveAsync();
}
