using Volo.Abp;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.MongoDB;

public static class CmsKitProMongoDbContextExtensions
{
    public static void ConfigureCmsKitPro(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureCmsKit();

        builder.Entity<NewsletterRecord>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "NewsletterRecords";
        });

        builder.Entity<ShortenedUrl>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "ShortenedUrls";
        });

        builder.Entity<Poll>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "Polls";
        });
        
        builder.Entity<PageFeedback>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "PageFeedbacks";
        });
        
        builder.Entity<PageFeedbackSetting>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "PageFeedbackSettings";
        });
        
        builder.Entity<FaqSection>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "FaqSections";
        });
        
        builder.Entity<FaqQuestion>(x =>
        {
            x.CollectionName = AbpCmsKitDbProperties.DbTablePrefix + "FaqQuestions";
        });
    }
}
