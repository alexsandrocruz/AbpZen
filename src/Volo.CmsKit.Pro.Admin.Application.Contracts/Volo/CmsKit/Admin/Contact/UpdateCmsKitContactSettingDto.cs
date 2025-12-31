using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.CmsKit.Admin.Contact;

[Serializable]
public class UpdateCmsKitContactSettingDto
{
    [Required]
    [EmailAddress]
    public string ReceiverEmailAddress { get; set; }
}
