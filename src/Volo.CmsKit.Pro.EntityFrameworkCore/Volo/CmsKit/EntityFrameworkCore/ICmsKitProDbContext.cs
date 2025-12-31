using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.EntityFrameworkCore;

[ConnectionStringName(AbpCmsKitDbProperties.ConnectionStringName)]
public interface ICmsKitProDbContext : IEfCoreDbContext
{
    DbSet<NewsletterRecord> NewsletterRecords { get; set; }
    DbSet<NewsletterPreference> NewsletterPreferences { get; set; }
    DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    DbSet<Poll> Polls { get; set; }
    DbSet<PollUserVote> PollUserVotes { get; set; }
    DbSet<PollOption> PollOptions { get; set; }
    DbSet<FaqQuestion> FaqQuestions { get; set; }
    DbSet<FaqSection> FaqSections { get; set; }
}