using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.ProposalBlockInstance.Dtos;

[Serializable]
public class CreateUpdateProposalBlockInstanceDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string BlockType { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProposalId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
