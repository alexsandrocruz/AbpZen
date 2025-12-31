using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.ProxyScripting.Generators.JQuery;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Public;
using Volo.CmsKit.Web;
using Volo.CmsKit.Web.Contents;

namespace Volo.CmsKit.Pro.Web;

[DependsOn(
    typeof(CmsKitCommonWebModule),
    typeof(CmsKitProCommonApplicationContractsModule)
    )]
public class CmsKitProCommonWebModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<CmsKitContentWidgetOptions>(options =>
        {
            options.AddWidget("Faq", "CmsFaq", "CmsFaqOptions");
            options.AddWidget("Poll", "CmsPollByCode", "CmsPolls");
            options.AddWidget("PageFeedback", "CmsPageFeedback", "CmsPageFeedbacks");
            options.AddWidget("PageFeedbackModal", "CmsPageFeedbackModal", "CmsPageFeedbackModals");
        });
        
        context.Services
            .AddSingleton(_ => new MarkdownPipelineBuilder()
                .UseAutoLinks()
                .UseBootstrap()
                .UseGridTables()
                .UsePipeTables()
                .Build());
        
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsKitProCommonWebModule>();
        });

        Configure<DynamicJavaScriptProxyOptions>(options =>
        {
            options.DisableModule(CmsKitProCommonRemoteServiceConsts.ModuleName);
        });
    }
}