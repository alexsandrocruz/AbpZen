using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.TextTemplating;
using Volo.CmsKit.Templates;

namespace Volo.CmsKit.PageFeedbacks;

public class PageFeedbackEmailSender : ITransientDependency
{
    protected IEmailSender EmailSender { get; }
    protected ITemplateRenderer TemplateRenderer { get; }
    protected ILogger<PageFeedbackEmailSender> Logger { get; }

    public PageFeedbackEmailSender(
        IEmailSender emailSender,
        ITemplateRenderer templateRenderer, 
        ILogger<PageFeedbackEmailSender> logger)
    {
        EmailSender = emailSender;
        TemplateRenderer = templateRenderer;
        Logger = logger;
    }

    public virtual async Task QueueAsync(PageFeedback pageFeedback, params string[] emailAddresses)
    {
        var body = await TemplateRenderer.RenderAsync(
            CmsKitEmailTemplates.PageFeedbackEmailTemplate,
            new {
                Title = "Page Feedback",
                EntityType = pageFeedback.EntityType,
                Url = pageFeedback.Url,
                IsUseful = pageFeedback.IsUseful,
                UserNote = pageFeedback.UserNote,
                CreationTime = pageFeedback.CreationTime
            }
        );

        foreach (var emailAddress in emailAddresses)
        {
            if (emailAddress.IsNullOrWhiteSpace())
            {
                Logger.LogWarning($"Email address is empty for page feedback setting with entity type: {pageFeedback.EntityType}");
                continue;
            }
            
            await EmailSender.QueueAsync(
                emailAddress.Trim(),
                "Page Feedback",
                body
            );
        }
    }
}