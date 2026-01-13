using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.Client.Dtos;

[Serializable]
public class CreateUpdateClientDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(256)]
    public string Name { get; set; } = string.Empty;
    [Required]
    public ClientType Type { get; set; }
    [StringLength(14)]
    public string Document { get; set; }
    [StringLength(256)]
    public string Email { get; set; }
    [StringLength(20)]
    public string Phone { get; set; }
    [StringLength(256)]
    public string Address { get; set; }
    [StringLength(256)]
    public string City { get; set; }
    [StringLength(2)]
    public string State { get; set; }
    [StringLength(2048)]
    public string Notes { get; set; }
    public bool? IsActive { get; set; }
    public string? LeadStatus { get; set; }
    public DateTime? FirstContactDate { get; set; }
    public DateTime? LastContactDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
