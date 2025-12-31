using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.Abp.Validation;

namespace Volo.Saas.Host.Dtos;

public class SaasTenantDatabaseConnectionStringsDto : ExtensibleEntityDto
{
    public string DatabaseName { get; set; }

    [DynamicStringLength(typeof(TenantConnectionStringConsts),nameof(TenantConnectionStringConsts.MaxValueLength))]
    [DisableAuditing]
    public string ConnectionString { get; set; }
}
