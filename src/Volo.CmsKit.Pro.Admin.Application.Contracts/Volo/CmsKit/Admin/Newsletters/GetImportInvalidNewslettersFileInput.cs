using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Newsletters;

public class GetImportInvalidNewslettersFileInput
{
    [Required] public string Token { get; set; }
}