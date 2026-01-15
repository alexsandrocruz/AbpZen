using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.SchedulerAvailability.Dtos;

[Serializable]
public class CreateUpdateSchedulerAvailabilityDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public int DayOfWeek { get; set; }
    [Required]
    public string StartTime { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SchedulerTypeId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
