using System;
using Volo.Abp.MultiTenancy;

namespace Volo.FileManagement.Directories;

[Serializable]
public class DirectoryDescriptorEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public string Name { get; set; }

    public Guid? ParentId { get; set; }
}
