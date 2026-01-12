using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Cursos.Client.Dtos;

[Serializable]
public class CreateUpdateClientDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    [StringLength(128)]
    public string Name { get; set; }
    [Required]
    [StringLength(128)]
    public string Email { get; set; }
    [StringLength(20)]
    public string Phone { get; set; }
    [Required]
    [StringLength(20)]
    public string CpfCnpj { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
