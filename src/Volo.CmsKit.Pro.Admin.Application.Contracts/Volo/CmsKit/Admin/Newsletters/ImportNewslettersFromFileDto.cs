namespace Volo.CmsKit.Admin.Newsletters;

public class ImportNewslettersFromFileDto
{
    public string EmailAddress { get; set; }

    public string Preference { get; set; }
    
    public string Source { get; set; }
    
    public string SourceUrl { get; set; }

    public override string ToString()
    {
        return $"{nameof(EmailAddress)}: {EmailAddress}, {nameof(Preference)}: {Preference}, {nameof(Source)}: {Source}, {nameof(SourceUrl)}: {SourceUrl}";
    }
}