using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Admin.Plans;

[Serializable]
public class GatewayPlanUpdateInput : ExtensibleObject
{
    [Required]
    public string ExternalId { get; set; }
}
