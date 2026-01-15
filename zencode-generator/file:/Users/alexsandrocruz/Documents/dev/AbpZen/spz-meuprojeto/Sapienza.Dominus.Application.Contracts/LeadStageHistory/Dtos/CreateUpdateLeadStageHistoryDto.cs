using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.LeadStageHistory.Dtos;

[Serializable]
public class CreateUpdateLeadStageHistoryDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? LeadId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
