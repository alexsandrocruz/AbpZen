namespace Volo.CmsKit.Pro.Web.Pages.Public.Shared.Components.PageFeedbacks;

public class PageFeedbackViewDto
{
    public string EntityType { get; set; }
    public string EntityId { get; set; }
    public string YesButtonText { get; set; }
    public string VeryHelpfulText { get; set; }
    public string NoButtonText { get; set; }
    public string NeedsImprovementText { get; set; }
    public string UserNotePlaceholder { get; set; }
    public string SubmitButtonText { get; set; }
    public bool ReverseButtons { get; set; }
    public string ThankYouMessageDescription { get; set; }
    public string ThankYouMessageTitle { get; set; }
    public bool HeaderVisible { get; set; } = true;
    public string HeaderText { get; set; }
    
    public PageFeedbackViewDto(string entityType, string entityId = null, string yesButtonText = null, string noButtonText = null, string userNotePlaceholder = null, string submitButtonText = null, bool reverseButtons = false, string thankYouMessageDescription = null, string thankYouMessageTitle = null, bool headerVisible = true, string headerText = null, string veryHelpfulText = null, string needsImprovementText = null)
    {
        EntityType = entityType;
        EntityId = entityId;
        YesButtonText = yesButtonText;
        NoButtonText = noButtonText;
        UserNotePlaceholder = userNotePlaceholder;
        SubmitButtonText = submitButtonText;
        ReverseButtons = reverseButtons;
        ThankYouMessageDescription = thankYouMessageDescription;
        ThankYouMessageTitle = thankYouMessageTitle;
        HeaderVisible = headerVisible;
        HeaderText = headerText;
        VeryHelpfulText = veryHelpfulText;
        NeedsImprovementText = needsImprovementText;
    }

    public PageFeedbackViewDto()
    {
        
    }
}