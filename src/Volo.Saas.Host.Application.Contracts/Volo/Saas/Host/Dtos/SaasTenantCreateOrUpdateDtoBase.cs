using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Volo.Saas.Host.Dtos;

public abstract class SaasTenantCreateOrUpdateDtoBase : ExtensibleObject
{
    [Required]
    [DynamicStringLength(typeof(TenantConsts),nameof(TenantConsts.MaxNameLength))]
    [Display(Name = "TenantName")]
    public string Name { get; set; }

    public Guid? EditionId { get; set; }

    public TenantActivationState ActivationState { get; set; }

    public DateTime? ActivationEndDate { get; set; }

    public DateTime? EditionEndDateUtc { get; set; }

    protected SaasTenantCreateOrUpdateDtoBase()
        : base(false)
    {

    }
}
