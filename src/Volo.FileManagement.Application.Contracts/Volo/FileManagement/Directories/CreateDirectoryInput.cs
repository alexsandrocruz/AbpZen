using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.FileManagement.Directories;

public class CreateDirectoryInput : ExtensibleObject
{
    public Guid? ParentId { get; set; }

    [Required]
    [DynamicStringLength(typeof(DirectoryDescriptorConsts), nameof(DirectoryDescriptorConsts.MaxNameLength))]
    public string Name { get; set; }

    public CreateDirectoryInput()
        : base(false)
    {

    }
}
