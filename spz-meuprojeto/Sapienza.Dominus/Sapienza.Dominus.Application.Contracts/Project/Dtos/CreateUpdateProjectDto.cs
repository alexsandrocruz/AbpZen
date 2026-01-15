using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Project.Dtos;

[Serializable]
public class CreateUpdateProjectDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public double Budget { get; set; }
    public DateTime DueDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
