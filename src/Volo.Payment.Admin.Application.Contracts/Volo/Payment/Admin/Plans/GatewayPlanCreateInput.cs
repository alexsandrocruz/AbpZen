using System.ComponentModel.DataAnnotations;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending;

namespace Volo.Payment.Admin.Plans;

public class GatewayPlanCreateInput : ExtensibleObject
{
    [Required]
    public string Gateway { get; set; }

    [Required]
    public string ExternalId { get; set; }
}
