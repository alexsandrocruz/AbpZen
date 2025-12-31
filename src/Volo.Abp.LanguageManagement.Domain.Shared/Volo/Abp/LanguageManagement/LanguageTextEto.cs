using System;
using JetBrains.Annotations;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.LanguageManagement;

[Serializable]
public class LanguageTextEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string ResourceName { get; set; }

    public string CultureName { get; set; }

    public string Name { get; set; }

    public string Value { get; set; }
}
