using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.GlobalFeatures;
using Volo.CmsKit.Faqs;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Newsletters;
using Volo.CmsKit.PageFeedbacks;
using Volo.CmsKit.Polls;
using Volo.CmsKit.UrlShorting;

namespace Volo.CmsKit.EntityFrameworkCore;

public static class CmsKitProDbContextModelCreatingExtensions
{
    public static void ConfigureCmsKitPro(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.ConfigureCmsKit();

        if (GlobalFeatureManager.Instance.IsEnabled<NewslettersFeature>())
        {
            builder.Entity<NewsletterRecord>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "NewsletterRecords", AbpCmsKitDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(n => n.EmailAddress).HasMaxLength(NewsletterRecordConst.MaxEmailAddressLength).IsRequired().HasColumnName(nameof(NewsletterRecord.EmailAddress));

                b.HasIndex(n => new { n.TenantId, n.EmailAddress });

                b.HasMany(n => n.Preferences).WithOne().HasForeignKey(x => x.NewsletterRecordId).IsRequired();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<NewsletterPreference>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "NewsletterPreferences", AbpCmsKitDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(n => n.Preference).HasMaxLength(NewsletterPreferenceConst.MaxPreferenceLength).IsRequired().HasColumnName(nameof(NewsletterPreference.Preference));
                b.Property(n => n.Source).HasMaxLength(NewsletterPreferenceConst.MaxSourceLength).IsRequired().HasColumnName(nameof(NewsletterPreference.Source));
                b.Property(n => n.SourceUrl).HasMaxLength(NewsletterPreferenceConst.MaxSourceUrlLength).IsRequired().HasColumnName(nameof(NewsletterPreference.SourceUrl));

                b.HasIndex(n => new { n.TenantId, n.Preference, n.Source });

                b.ApplyObjectExtensionMappings();
            });
        }

        if (GlobalFeatureManager.Instance.IsEnabled<UrlShortingFeature>())
        {
            builder.Entity<ShortenedUrl>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "ShortenedUrls", AbpCmsKitDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.Property(n => n.Source).HasMaxLength(ShortenedUrlConst.MaxSourceLength).IsRequired().HasColumnName(nameof(ShortenedUrl.Source));
                b.Property(n => n.Target).HasMaxLength(ShortenedUrlConst.MaxTargetLength).IsRequired().HasColumnName(nameof(ShortenedUrl.Target));

                b.HasIndex(n => new { n.TenantId, n.Source });

                b.ApplyObjectExtensionMappings();
            });
        }

        if (GlobalFeatureManager.Instance.IsEnabled<PollsFeature>())
        {
            builder.Entity<Poll>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "Polls", AbpCmsKitDbProperties.DbSchema);

                b.Property(n => n.Question).HasMaxLength(PollConst.MaxQuestionLength).IsRequired().HasColumnName(nameof(Poll.Question));
                b.Property(n => n.Code).HasMaxLength(PollConst.MaxCodeLength).IsRequired().HasColumnName(nameof(Poll.Code));
                b.Property(n => n.Widget).HasMaxLength(PollConst.MaxWidgetNameLength).HasColumnName(nameof(Poll.Widget));
                b.Property(n => n.Name).HasMaxLength(PollConst.MaxNameLength).HasColumnName(nameof(Poll.Name));

                b.ConfigureByConvention();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<PollUserVote>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "PollUserVotes", AbpCmsKitDbProperties.DbSchema);

                b.ConfigureByConvention();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<PollOption>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "PollOptions", AbpCmsKitDbProperties.DbSchema);

                b.Property(n => n.Text).HasMaxLength(PollConst.MaxTextLength).IsRequired().HasColumnName(nameof(PollOption.Text));

                b.ConfigureByConvention();

                b.ApplyObjectExtensionMappings();
            });
        }
        
        if (GlobalFeatureManager.Instance.IsEnabled<PageFeedbackFeature>())
        {
            builder.Entity<PageFeedback>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "PageFeedbacks", AbpCmsKitDbProperties.DbSchema);

                b.Property(n => n.Url).HasMaxLength(PageFeedbackConst.MaxUrlLength).HasColumnName(nameof(PageFeedback.Url));
                b.Property(n => n.EntityType).HasMaxLength(PageFeedbackConst.MaxEntityTypeLength).IsRequired().HasColumnName(nameof(PageFeedback.EntityType));
                b.Property(n => n.EntityId).HasMaxLength(PageFeedbackConst.MaxEntityIdLength).HasColumnName(nameof(PageFeedback.EntityId));
                b.Property(n => n.UserNote).HasMaxLength(PageFeedbackConst.MaxUserNoteLength).HasColumnName(nameof(PageFeedback.UserNote));
                b.Property(n => n.AdminNote).HasMaxLength(PageFeedbackConst.MaxAdminNoteLength).HasColumnName(nameof(PageFeedback.AdminNote));
                b.Property(n => n.IsUseful).HasColumnName(nameof(PageFeedback.IsUseful));
                b.Property(n => n.IsHandled).HasColumnName(nameof(PageFeedback.IsHandled));
                b.Property(n => n.FeedbackUserId).HasColumnName(nameof(PageFeedback.FeedbackUserId));

                b.ConfigureByConvention();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<PageFeedbackSetting>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "PageFeedbackSettings", AbpCmsKitDbProperties.DbSchema);
                
                b.Property(n => n.EntityType).HasMaxLength(PageFeedbackConst.MaxEntityTypeLength).HasColumnName(nameof(PageFeedbackSetting.EntityType));
                b.Property(n => n.EmailAddresses).HasMaxLength(PageFeedbackConst.MaxEmailAddressesLength).HasColumnName(nameof(PageFeedbackSetting.EmailAddresses));
                
                b.HasIndex(n => new { n.TenantId, n.EntityType }).IsUnique();
                
                b.ConfigureByConvention();
                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<FaqSection>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "FaqSections", AbpCmsKitDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(n => n.Name).HasMaxLength(FaqSectionConst.MaxNameLength).IsRequired();
                b.Property(n => n.GroupName).HasMaxLength(FaqSectionConst.MaxGroupNameLength).IsRequired();
                b.Property(n => n.Order).IsRequired();

                b.ApplyObjectExtensionMappings();
            });

            builder.Entity<FaqQuestion>(b =>
            {
                b.ToTable(AbpCmsKitDbProperties.DbTablePrefix + "FaqQuestions", AbpCmsKitDbProperties.DbSchema);
                b.ConfigureByConvention();
                b.Property(n => n.SectionId).IsRequired();
                b.Property(n => n.Title).HasMaxLength(FaqQuestionConst.MaxTitleLength).IsRequired();
                b.Property(n => n.Text).HasMaxLength(FaqQuestionConst.MaxTextLength).IsRequired();
                b.Property(n => n.Order).IsRequired();

                b.ApplyObjectExtensionMappings();
            });
        }

        builder.TryConfigureObjectExtensions<CmsKitProDbContext>();
    }
}
