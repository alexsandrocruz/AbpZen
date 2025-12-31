using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity;

public class GetIdentityUserListAsFileInput : GetIdentityUsersInput
{
    [Required]
    public string Token { get; set; }
}