using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.Location.Dtos;

[Serializable]
public class CreateUpdateLocationDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;
    [StringLength(256)]
    public string Address { get; set; }
    [StringLength(256)]
    public string City { get; set; }
    [StringLength(2)]
    public string State { get; set; }
    public int? Capacity { get; set; }
    public string ZipCode { get; set; }
    public string Notes { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
