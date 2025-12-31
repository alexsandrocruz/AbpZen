using System.Threading.Tasks;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Renderers;

public interface IMarkdownToHtmlRenderer
{
    string Render(string rawMarkdown, bool allowHtmlTags = true, bool preventXSS = true);
}