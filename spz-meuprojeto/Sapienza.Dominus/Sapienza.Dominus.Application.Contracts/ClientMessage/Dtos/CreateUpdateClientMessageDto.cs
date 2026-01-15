using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.ClientMessage.Dtos;

[Serializable]
public class CreateUpdateClientMessageDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Channel { get; set; }
    [Required]
    public string Direction { get; set; }
    public string Subject { get; set; }
    [Required]
    public string Content { get; set; }
    public DateTime SentAt { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
