using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.MongoDB;

[ConnectionStringName(AbpCmsKitDbProperties.ConnectionStringName)]
public class CmsKitProMongoDbContext : AbpMongoDbContext, ICmsKitProMongoDbContext
{
    public IMongoCollection<NewsletterRecord> NewsletterRecords => Collection<NewsletterRecord>();
    public IMongoCollection<ShortenedUrl> ShortenedUrls => Collection<ShortenedUrl>();
    public IMongoCollection<Poll> Polls => Collection<Poll>();
    public IMongoCollection<PollUserVote> PollUserVotes => Collection<PollUserVote>();
    public IMongoCollection<PageFeedback> PageFeedbacks => Collection<PageFeedback>();
    public IMongoCollection<PageFeedbackSetting> PageFeedbackSettings => Collection<PageFeedbackSetting>();
    public IMongoCollection<FaqSection> FaqSections => Collection<FaqSection>();
    public IMongoCollection<FaqQuestion> FaqQuestions => Collection<FaqQuestion>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureCmsKitPro();
    }
}
