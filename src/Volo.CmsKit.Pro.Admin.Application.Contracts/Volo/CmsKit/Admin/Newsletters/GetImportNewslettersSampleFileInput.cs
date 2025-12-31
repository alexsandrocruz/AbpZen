using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Newsletters;

public class GetImportNewslettersSampleFileInput
{
    [Required] public string Token { get; set; }
}