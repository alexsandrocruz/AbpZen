using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Identity;

public class GetIdentitySessionListInput : ExtensiblePagedAndSortedResultRequestDto
{
    public Guid UserId { get; set; }

    public string Device { get; set; }

    public string ClientId { get; set; }
}
