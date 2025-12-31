using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Admin.Plans;

[Serializable]
public class PlanCreateInput : ExtensibleObject
{
    [Required]
    public string Name { get; set; }
}
