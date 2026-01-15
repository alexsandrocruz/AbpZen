using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.SchedulerType.Dtos;

[Serializable]
public class CreateUpdateSchedulerTypeDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int DurationMinutes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
