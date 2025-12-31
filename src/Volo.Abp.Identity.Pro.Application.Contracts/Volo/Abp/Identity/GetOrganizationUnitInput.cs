using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetOrganizationUnitInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
