using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.FileManagement.Files;

public class CreateFileInputWithStream : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(FileDescriptorConsts), nameof(FileDescriptorConsts.MaxNameLength))]
    public string Name { get; set; }

    public IRemoteStreamContent File { get; set; }

    public bool OverrideExisting { get; set; }

    public CreateFileInputWithStream()
        : base(false)
    {
        OverrideExisting = true;
    }
}
