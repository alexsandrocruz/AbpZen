namespace Volo.Abp.Identity;

public class InvalidImportUsersFromFileDto : ImportUsersFromFileDto
{
    public string ErrorReason { get; set; }
}