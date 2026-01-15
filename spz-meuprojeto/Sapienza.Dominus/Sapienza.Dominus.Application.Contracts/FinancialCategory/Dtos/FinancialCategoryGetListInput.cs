using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.FinancialCategory.Dtos;

[Serializable]
public class FinancialCategoryGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
