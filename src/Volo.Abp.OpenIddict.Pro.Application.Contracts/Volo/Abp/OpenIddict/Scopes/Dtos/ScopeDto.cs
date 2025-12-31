using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.OpenIddict.Scopes.Dtos;

public class ScopeDto : ExtensibleEntityDto<Guid>
{
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public bool BuildIn { get; set; }

    public HashSet<string> Resources { get; set; }
}
