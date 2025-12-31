using System;
using Volo.Abp.Domain.Entities;

namespace Volo.FileManagement.Files;

public class MoveFileInput : IHasConcurrencyStamp
{
    public Guid Id { get; set; }

    public Guid? NewDirectoryId { get; set; }

    public string ConcurrencyStamp { get; set; }
}
