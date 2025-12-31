using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Abp.OpenIddict.Scopes.Dtos;

public class ScopeCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [RegularExpression(@"\w+", ErrorMessage = "TheScopeNameCannotContainSpaces")]
    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public HashSet<string> Resources { get; set; }

    public ScopeCreateOrUpdateDtoBase()
        : base(false)
    {

    }
}
