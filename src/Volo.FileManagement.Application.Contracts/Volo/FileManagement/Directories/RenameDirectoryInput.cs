using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Validation;

namespace Volo.FileManagement.Directories;

public class RenameDirectoryInput : IHasConcurrencyStamp
{
    [Required]
    [DynamicStringLength(typeof(DirectoryDescriptorConsts), nameof(DirectoryDescriptorConsts.MaxNameLength))]
    public string Name { get; set; }

    public string ConcurrencyStamp { get; set; }
}
