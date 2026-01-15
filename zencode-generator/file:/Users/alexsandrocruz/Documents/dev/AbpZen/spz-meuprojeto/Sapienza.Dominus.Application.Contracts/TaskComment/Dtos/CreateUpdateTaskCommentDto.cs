using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.TaskComment.Dtos;

[Serializable]
public class CreateUpdateTaskCommentDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Content { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? TaskId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
