using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.FileManagement.Directories;

// TODO: Missing [Serializable] attribute check for dtos in entire solution.
public class DirectoryDescriptorDto :  ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public Guid? ParentId { get; set; }

    public string ConcurrencyStamp { get; set; }

    public DirectoryDescriptorDto()
        :base(false)
    {
    }
}
