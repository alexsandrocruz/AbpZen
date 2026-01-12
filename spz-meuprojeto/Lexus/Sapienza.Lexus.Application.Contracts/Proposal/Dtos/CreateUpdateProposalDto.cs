#nullable enable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sapienza.Lexus.PropostalItem.Dtos;

namespace Sapienza.Lexus.Proposal.Dtos;

[Serializable]
public class CreateUpdateProposalDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string? Number { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? Validate { get; set; }
    public string? Obs { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
    public List<CreateUpdatePropostalItemDto>? PropostalItems { get; set; }
}
