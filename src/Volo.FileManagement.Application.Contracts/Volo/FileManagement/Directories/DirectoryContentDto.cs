using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.FileManagement.Files;

namespace Volo.FileManagement.Directories;

public class DirectoryContentDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string Name { get; set; }

    public bool IsDirectory { get; set; }

    public long Size { get; set; }

    public FileIconInfo IconInfo { get; set; }

    public string ConcurrencyStamp { get; set; }
}
