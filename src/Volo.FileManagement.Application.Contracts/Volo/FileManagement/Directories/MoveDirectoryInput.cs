using System;
using Volo.Abp.Domain.Entities;

namespace Volo.FileManagement.Directories;

public class MoveDirectoryInput : IHasConcurrencyStamp
{
    public Guid Id { get; set; }

    public Guid? NewParentId { get; set; }

    public string ConcurrencyStamp { get; set; }
}
