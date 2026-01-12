using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;

namespace Sapienza.Lexus.Lawyer.Dtos;

[Serializable]
public class CreateUpdateLawyerDto : IHasConcurrencyStamp
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(128)]
    public string FullName { get; set; }
    [StringLength(64)]
    public string PreferredName { get; set; }
    public string ConcurrencyStamp { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
