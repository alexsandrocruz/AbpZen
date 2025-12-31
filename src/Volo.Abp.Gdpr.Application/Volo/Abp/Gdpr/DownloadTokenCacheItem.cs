using System;

namespace Volo.Abp.Gdpr;

[Serializable]
public class DownloadTokenCacheItem
{
    public Guid RequestId { get; set; }
}