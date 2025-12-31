using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Identity;

public class GetImportUsersSampleFileInput
{
    [Range(1,2)]
    public ImportUsersFromFileType FileType { get; set; }

    [Required]
    public string Token { get; set; }
}