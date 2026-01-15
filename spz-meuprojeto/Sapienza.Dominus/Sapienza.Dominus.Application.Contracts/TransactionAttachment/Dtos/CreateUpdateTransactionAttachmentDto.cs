using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.TransactionAttachment.Dtos;

[Serializable]
public class CreateUpdateTransactionAttachmentDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string FileName { get; set; }
    [Required]
    public string FileUrl { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? TransactionId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
