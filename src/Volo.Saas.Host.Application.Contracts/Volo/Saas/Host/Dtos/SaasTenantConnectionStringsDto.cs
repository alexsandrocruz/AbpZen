using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Volo.Saas.Host.Dtos;

public class SaasTenantConnectionStringsDto : ExtensibleEntityDto
{
    [DynamicStringLength(typeof(TenantConnectionStringConsts),nameof(TenantConnectionStringConsts.MaxValueLength))]
    [DisableAuditing]
    public string Default { get; set; }

    public List<SaasTenantDatabaseConnectionStringsDto> Databases { get; set; }
}
