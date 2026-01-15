using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Dominus.Budget.Dtos;

[Serializable]
public class CreateUpdateBudgetDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public int Month { get; set; }
    [Required]
    public double Amount { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? FinancialCategoryId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
