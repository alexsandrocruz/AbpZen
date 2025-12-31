using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Saas.Host.Dtos;

public class EditionLookupDto : ExtensibleEntityDto<Guid>
{
    public string DisplayName { get; set; }
}
