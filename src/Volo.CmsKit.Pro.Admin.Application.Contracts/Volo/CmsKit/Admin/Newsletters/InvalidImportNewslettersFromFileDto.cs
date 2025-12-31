namespace Volo.CmsKit.Admin.Newsletters;

public class InvalidImportNewslettersFromFileDto : ImportNewslettersFromFileDto
{
    public string ErrorReason { get; set; }
}