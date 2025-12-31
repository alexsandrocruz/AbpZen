using System;
using Volo.Abp.MultiTenancy;

namespace Volo.FileManagement.Files;

[Serializable]
[IgnoreMultiTenancy]
public class FileDownloadTokenCacheItem
{
    public Guid FileDescriptorId { get; set; }
    
    public Guid? TenantId { get; set; }
}
