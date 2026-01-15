using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.TimeEntry.Dtos;

[Serializable]
public class CreateUpdateTimeEntryDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Hours { get; set; }
    [Required]
    public DateTime Date { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
