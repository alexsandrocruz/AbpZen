using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace Volo.Abp.Identity;

public class ImportUsersFromFileInputWithStream
{
    public IRemoteStreamContent File { get; set; }
    
    [Range(1,2)]
    public ImportUsersFromFileType FileType { get; set; }
}
