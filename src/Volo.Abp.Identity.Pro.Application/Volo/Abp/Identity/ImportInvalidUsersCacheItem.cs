using System;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace Volo.Abp.Identity;

[Serializable]
[IgnoreMultiTenancy]
public class ImportInvalidUsersCacheItem : IDownloadCacheItem
{
    public List<InvalidImportUsersFromFileDto> InvalidUsers { get; set; }

    public ImportUsersFromFileType FileType { get; set; }
    
    public string Token { get; set; }
    
    public Guid? TenantId { get; set; }
}