using System;
using Volo.Abp.Domain.Entities;

namespace Volo.Payment.Admin.Plans;

[Serializable]
public class PlanUpdateInput : PlanCreateInput, IHasConcurrencyStamp
{
    public string ConcurrencyStamp { get; set; }
}
