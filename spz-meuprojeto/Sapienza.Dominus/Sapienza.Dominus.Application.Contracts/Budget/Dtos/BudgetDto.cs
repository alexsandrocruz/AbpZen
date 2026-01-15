using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Budget.Dtos;

[Serializable]
public class BudgetDto : FullAuditedEntityDto<Guid>
{
    public int Year { get; set; }
    public int Month { get; set; }
    public double Amount { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? FinancialCategoryId { get; set; }
    public string? FinancialCategoryDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
