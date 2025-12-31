using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Public.Contact;

public class ContactCreateInput
{
    public string ContactName { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Message { get; set; }

    public string RecaptchaToken { get; set; }
}
