using System;
using Volo.Abp.MultiTenancy;

namespace Volo.FileManagement.Files;

[Serializable]
public class FileDescriptorEto : IMultiTenant
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    public Guid? DirectoryId { get; set; }

    public string Name { get; set; }

    public string MimeType { get; set; }

    public long Size { get; set; }
}
