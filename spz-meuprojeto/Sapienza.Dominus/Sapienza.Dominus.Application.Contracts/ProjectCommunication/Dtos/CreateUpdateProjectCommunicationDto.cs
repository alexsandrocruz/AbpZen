using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.ProjectCommunication.Dtos;

[Serializable]
public class CreateUpdateProjectCommunicationDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Channel { get; set; }
    public string Subject { get; set; }
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ProjectId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
