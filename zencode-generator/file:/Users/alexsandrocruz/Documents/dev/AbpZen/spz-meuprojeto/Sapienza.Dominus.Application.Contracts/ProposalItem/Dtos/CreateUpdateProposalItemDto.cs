using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.ProposalItem.Dtos;

[Serializable]
public class CreateUpdateProposalItemDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Quantity { get; set; }
    [Required]
    public double Price { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
