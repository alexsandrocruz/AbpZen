using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity;

public class GetImportInvalidUsersFileInput
{
    [Required]
    public string Token { get; set; }
}