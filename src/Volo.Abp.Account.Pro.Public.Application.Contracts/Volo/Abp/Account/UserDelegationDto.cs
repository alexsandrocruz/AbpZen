using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Account;

public class UserDelegationDto : EntityDto<Guid>
{
    public string UserName { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
}
