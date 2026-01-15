using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Booking.Dtos;

[Serializable]
public class CreateUpdateBookingDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string ClientName { get; set; }
    [Required]
    public string ClientEmail { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? SchedulerTypeId { get; set; }
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
