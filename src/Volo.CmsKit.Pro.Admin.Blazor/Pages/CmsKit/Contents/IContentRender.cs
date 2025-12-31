using System.Threading.Tasks;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

public interface IContentRender
{
    Task<string> RenderAsync(string content);
}
