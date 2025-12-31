using System;
using Volo.Abp.Application.Dtos;

namespace Volo.FileManagement.Directories;

public class DirectoryContentRequestInput : PagedAndSortedResultRequestDto
{
    public string Filter { get; set; }

    public Guid? Id { get; set; }
}
