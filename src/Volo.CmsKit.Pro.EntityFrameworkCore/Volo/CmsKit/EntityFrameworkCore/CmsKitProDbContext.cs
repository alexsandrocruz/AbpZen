using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.EntityFrameworkCore;

[ConnectionStringName(AbpCmsKitDbProperties.ConnectionStringName)]
public class CmsKitProDbContext : AbpDbContext<CmsKitProDbContext>, ICmsKitProDbContext
{
    public DbSet<NewsletterRecord> NewsletterRecords { get; set; }
    public DbSet<NewsletterPreference> NewsletterPreferences { get; set; }
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<Poll> Polls { get; set; }
    public DbSet<PollUserVote> PollUserVotes { get; set; }
    public DbSet<PollOption> PollOptions { get; set; }
    public DbSet<PageFeedback> PageFeedbacks { get; set; }
    public DbSet<PageFeedbackSetting> PageFeedbackSettings { get; set; }
    public DbSet<FaqQuestion> FaqQuestions { get; set; }
    public DbSet<FaqSection> FaqSections { get; set; }

    public CmsKitProDbContext(DbContextOptions<CmsKitProDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureCmsKitPro();
    }
}
