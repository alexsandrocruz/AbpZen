namespace Volo.CmsKit.Admin.Newsletters;

public class GetNewsletterRecordsCsvRequestInput
{
    public string Preference { get; set; }

    public string Source { get; set; }
    
    public string EmailAddress { get; set; }

    public string Token { get; set; }
}
