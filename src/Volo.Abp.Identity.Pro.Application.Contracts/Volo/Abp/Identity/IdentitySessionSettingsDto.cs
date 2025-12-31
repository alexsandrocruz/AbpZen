using System.ComponentModel.DataAnnotations;
using Volo.Abp.Identity.Settings;

namespace Volo.Abp.Identity;

public class IdentitySessionSettingsDto
{
    [Display(Name = "DisplayName:Abp.Identity.PreventConcurrentLogin")]
    public IdentityProPreventConcurrentLoginBehaviour PreventConcurrentLogin { get; set; }
}
