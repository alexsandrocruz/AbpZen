using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.Saas.Host.Dtos;

public class SaasTenantDatabasesDto : ExtensibleEntityDto
{
    public List<string> Databases { get; set; }
}
