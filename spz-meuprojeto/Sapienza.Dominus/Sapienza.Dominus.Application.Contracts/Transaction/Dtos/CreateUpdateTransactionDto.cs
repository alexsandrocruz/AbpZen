using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Transaction.Dtos;

[Serializable]
public class CreateUpdateTransactionDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Amount { get; set; }
    [Required]
    public string Type { get; set; }
    public string Status { get; set; }
    [Required]
    public DateTime DueDate { get; set; }
    public DateTime PaymentDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public Guid? FinancialCategoryId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
