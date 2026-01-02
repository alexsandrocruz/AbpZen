using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeptonXDemoApp.Edital;

/// <summary>
/// Edital entity
/// </summary>
public class Edital : FullAuditedAggregateRoot<Guid>
{
    public string? Objeto { get; set; }
    public DateTime? Data { get; set; }
    public decimal? Valor { get; set; }

    protected Edital()
    {
        // Required by EF Core
    }

    public Edital(Guid id) : base(id)
    {
    }
}
