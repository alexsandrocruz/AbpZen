using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Dominus.Transaction.Dtos;

[Serializable]
public class TransactionDto : FullAuditedEntityDto<Guid>
{
    public string Description { get; set; }
    public double Amount { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime PaymentDate { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ClientId { get; set; }
    public string? ClientDisplayName { get; set; }
    public Guid? FinancialCategoryId { get; set; }
    public string? FinancialCategoryDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
