using System;
using Volo.Abp.Auditing;

namespace Volo.Saas;

[Serializable]
public class EditionEto : IHasEntityVersion
{
    public Guid Id { get; set; }

    public string DisplayName { get; set; }

    public int EntityVersion { get; set; }
}
