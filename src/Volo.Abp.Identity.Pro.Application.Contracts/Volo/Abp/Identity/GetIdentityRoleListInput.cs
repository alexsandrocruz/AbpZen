using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetIdentityRoleListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public string Filter { get; set; }
}
