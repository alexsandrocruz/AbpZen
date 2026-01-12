using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.PropostalItem.Dtos;

[Serializable]
public class CreateUpdatePropostalItemDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string Desc { get; set; }
    public decimal? Quant { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? Total { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
