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
public interface ICmsKitProMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<NewsletterRecord> NewsletterRecords { get; }
    IMongoCollection<ShortenedUrl> ShortenedUrls { get; }
    IMongoCollection<Poll> Polls { get; }
    IMongoCollection<PollUserVote> PollUserVotes { get; }
    IMongoCollection<PageFeedbackSetting> PageFeedbackSettings { get; }
    IMongoCollection<PageFeedback> PageFeedbacks { get; }
    IMongoCollection<FaqSection> FaqSections { get; }
    IMongoCollection<FaqQuestion> FaqQuestions { get; }
}
