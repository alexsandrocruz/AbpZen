using System;

namespace Volo.CmsKit.Public.Emailing.Templates;

public class NewsletterEmailBodyModel
{
    public string Root { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Text { get; set; }
    public string Email { get; set; }
    public string Code { get; set; }
}
