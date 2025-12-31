using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Volo.FileManagement.Files;

public class RenameFileInput : IHasConcurrencyStamp
{
    [Required]
    [DynamicStringLength(typeof(FileDescriptorConsts), nameof(FileDescriptorConsts.MaxNameLength))]
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }
}
