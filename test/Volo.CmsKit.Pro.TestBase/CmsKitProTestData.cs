using System;
using System.Collections.Generic;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Pro;

public class CmsKitProTestData : ISingletonDependency
{
    public Guid NewsletterEmailId { get; } = Guid.NewGuid();

    public string Email { get; } = "info@abp.io";

    public string Preference { get; } = "Community";

    public string Source { get; } = "Community.ArticleRead";

    public Guid ShortenedUrlId1 { get; } = Guid.NewGuid();

    public string ShortenedUrlSource1 { get; } = "/SomeUrl";

    public string ShortenedUrlTarget1 { get; } = "Blog/51235-12354-21323-a2412";

    public Guid ShortenedUrlId2 { get; } = Guid.NewGuid();

    public string ShortenedUrlSource2 { get; } = "SomeUrl2";

    public string ShortenedUrlTarget2 { get; } = "Docs/51235-12354-5234-a2412";

    public Guid PollId { get; } = Guid.NewGuid();
    public Guid PollOptionId { get; } = Guid.NewGuid();
    public Guid PollOptionId2 { get; } = Guid.NewGuid();

    public string Question { get; } = "What is your question?";
    public string Widget { get; } = "poll-left";
    public string Code { get; } = "pollcode";
    public string Name { get; } = "poll-name-readable";

    public string EntityType1 { get; } = "EntityType1";
    public string EntityType2 { get; } = "EntityType2";
    public string EntityId1 { get; } = "1";
    public string EntityId2 { get; } = "2";
    public string NullEntityId { get; } = null;
    public List<string> FallBackEmailAddresses { get; } = new(){"falback@mail.com"};
    public string EmailAddresses2 { get; } = "test2@mail.com";
    public string EmailAddresses { get; } = "test@mail.com";
    
    public Guid User1Id { get; } = Guid.NewGuid();
    public Guid User2Id { get; } = Guid.NewGuid();

    public Guid FaqSectionId { get; } = Guid.NewGuid();
    public string FaqSectionName { get; } = "faq-section-name-readable";
    public string FaqSectionGroupName { get; } = "faq-section-group-name-readable";
    public int FaqSectionOrder { get; } = 0;
    public Guid FaqQuestionId { get; } = Guid.NewGuid();
    public string FaqQuestionTitle { get; } = "faq-question-title-readable";
    public string FaqQuestionText { get; } = "faq-question-text-readable";
    public int FaqQuestionOrder { get; } = 0;
    
    public Guid PageFeedbackUserId { get; } = Guid.NewGuid();
}
