using System;

namespace Volo.Abp.Identity;

[Serializable]
public class IdentitySessionCacheItem
{
    public Guid Id { get; set; }

    public virtual string SessionId  { get; set; }

    public virtual string IpAddress  { get; set; }

    public virtual DateTime? CacheLastAccessed  { get; set; }

    public virtual int HitCount  { get; set; }
}
