using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Transaction.Dtos;

[Serializable]
public class TransactionGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
    public Guid? ClientId { get; set; }
    public Guid? FinancialCategoryId { get; set; }
}
