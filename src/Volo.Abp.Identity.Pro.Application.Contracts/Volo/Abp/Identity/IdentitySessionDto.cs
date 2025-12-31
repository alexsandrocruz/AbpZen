using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class IdentitySessionDto : ExtensibleEntityDto<Guid>
{
    public virtual string SessionId { get; set; }

    public virtual bool IsCurrent { get; set; }

    public virtual string Device { get; set; }

    public virtual string DeviceInfo { get; set; }

    public virtual Guid? TenantId { get; set; }

    public virtual string TenantName { get; set; }

    public virtual Guid UserId { get; set; }

    public virtual string UserName { get; set; }

    public virtual string ClientId { get; set; }

    public virtual string[] IpAddresses { get; set; }

    public virtual DateTime SignedIn { get; set; }

    public virtual DateTime? LastAccessed { get; set; }
}
