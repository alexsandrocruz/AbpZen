using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Cursos.LegalProcess.Dtos;

[Serializable]
public class CreateUpdateLegalProcessDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(32)]
    public string ProcessNumber { get; set; }
    [Required]
    [StringLength(128)]
    public string Title { get; set; }
    [StringLength(2048)]
    public string Description { get; set; }
    [Required]
    public DateTime DateOpened { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    [Required]
    public Guid LawyerId { get; set; }
    [Required]
    public Guid ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
