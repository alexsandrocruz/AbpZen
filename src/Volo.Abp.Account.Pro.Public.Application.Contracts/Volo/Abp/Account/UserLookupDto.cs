using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Account;

public class UserLookupDto: EntityDto<Guid>
{
    public string UserName { get; set; }
}