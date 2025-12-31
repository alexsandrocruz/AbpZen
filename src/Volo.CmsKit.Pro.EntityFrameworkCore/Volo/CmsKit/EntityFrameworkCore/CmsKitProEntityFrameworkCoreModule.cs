using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Polls;
using Volo.CmsKit.Tags;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.EntityFrameworkCore;

[DependsOn(
    typeof(CmsKitProDomainModule),
    typeof(CmsKitEntityFrameworkCoreModule)
)]
public class CmsKitProEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CmsKitProDbContext>(options =>
        {
            options.AddRepository<NewsletterRecord, EfCoreNewsletterRecordRepository>();
            options.AddRepository<ShortenedUrl, EfCoreShortenedUrlRepository>();
            options.AddRepository<Poll, EfCorePollRepository>();
            options.AddRepository<FaqSection, EfCoreFaqSectionRepository>();
            options.AddRepository<FaqQuestion, EfCoreFaqQuestionRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
