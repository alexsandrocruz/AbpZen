using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.EventCommission.Dtos;

[Serializable]
public class CreateUpdateEventCommissionDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal? Value { get; set; }
    public decimal? Percentage { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? EventId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
