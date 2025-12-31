using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Payment.Requests;

namespace Volo.Payment.Requests;

[Serializable]
public class PaymentRequestProductCreateDto
{
    [Required]
    public string Code { get; set; }

    [Required]
    public string Name { get; set; }

    public float UnitPrice { get; set; }

    [Range(1, int.MaxValue)]
    public int Count { get; set; }

    public float? TotalPrice { get; set; }

    public PaymentType PaymentType { get; set; }

    public Guid? PlanId { get; set; }

    public Dictionary<string, IPaymentRequestProductExtraParameterConfiguration> ExtraProperties { get; set; }

    public PaymentRequestProductCreateDto()
    {
        ExtraProperties = new Dictionary<string, IPaymentRequestProductExtraParameterConfiguration>();
    }
}
