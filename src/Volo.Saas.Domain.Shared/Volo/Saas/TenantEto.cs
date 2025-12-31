using System;
using Volo.Abp.Auditing;

namespace Volo.Saas;

[Serializable]
public class TenantEto : IHasEntityVersion
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid? EditionId { get; set; }

    public DateTime? EditionEndDateUtc { get; set; }

    public int EntityVersion { get; set; }
}
