using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.MongoDB;

[DependsOn(
    typeof(CmsKitProDomainModule),
    typeof(CmsKitMongoDbModule)
    )]
public class CmsKitProMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<CmsKitProMongoDbContext>(options =>
        {
            options.AddRepository<NewsletterRecord, MongoNewsletterRecordRepository>();
            options.AddRepository<ShortenedUrl, MongoShortenedUrlRepository>();
            options.AddRepository<Poll, MongoPollRepository>();
            options.AddRepository<PollUserVote, MongoPollUserVoteRepository>();
            options.AddRepository<PageFeedbackSetting, MongoPageFeedbackSettingRepository>();
            options.AddRepository<PageFeedback, MongoPageFeedbackRepository>();
            options.AddRepository<FaqSection, MongoFaqSectionRepository>();
            options.AddRepository<FaqQuestion, MongoFaqQuestionRepository>();
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        
    }
}
