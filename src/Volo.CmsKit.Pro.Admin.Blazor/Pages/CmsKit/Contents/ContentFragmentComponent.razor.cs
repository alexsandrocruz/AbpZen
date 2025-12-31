using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Renderers;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

public partial class ContentFragmentComponent
{
    [Parameter]
    public IContent ContentDto { get; set; }

    [Inject]
    public IMarkdownToHtmlRenderer MarkdownRenderer { get; set; }

    [Inject]
    public IOptions<CmsKitContentWidgetOptions> Options { get; set; }

    public virtual string RenderMarkdown(string content)
    {
        return MarkdownRenderer.Render(content);
    }
}
