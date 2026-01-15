using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Contract.Dtos;

[Serializable]
public class CreateUpdateContractDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Content { get; set; }
    public string Status { get; set; }
    public double TotalValue { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
