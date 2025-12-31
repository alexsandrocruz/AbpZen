using System;
using System.Threading.Tasks;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;
using Volo.CmsKit.Contents;

namespace Volo.CmsKit.Pro.Admin.Blazor.Pages.CmsKit.Contents;

public class ContentRender : IContentRender, IScopedDependency
{
    protected IServiceProvider ServiceProvider { get; }

    protected ContentParser ContentParser { get; }

    protected TestContext RenderContext { get; set; }

    public ContentRender(
        IServiceProvider serviceProvider,
        ContentParser contentParser)
    {
        ServiceProvider = serviceProvider;
        ContentParser = contentParser;
        InitRenderContext();
    }

    public virtual async Task<string> RenderAsync(string content)
    {
        var contentDto = new DefaultContentDto { ContentFragments = await ContentParser.ParseAsync(content) };
        var contentFragment = RenderContext.RenderComponent<ContentFragmentComponent>(
            parameters => parameters
                .Add(p => p.ContentDto, contentDto));

        return contentFragment.Markup;
    }

    private void InitRenderContext()
    {
        RenderContext = new TestContext();
        RenderContext.Services.AddSingleton(typeof(IOptions<CmsKitContentWidgetOptions>),ServiceProvider.GetService<IOptions<CmsKitContentWidgetOptions>>() );
        RenderContext.Services.AddFallbackServiceProvider(ServiceProvider);
    }
}
