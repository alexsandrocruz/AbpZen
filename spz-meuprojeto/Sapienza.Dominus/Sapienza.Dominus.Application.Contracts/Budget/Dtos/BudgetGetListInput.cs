using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Budget.Dtos;

[Serializable]
public class BudgetGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? FinancialCategoryId { get; set; }
}
