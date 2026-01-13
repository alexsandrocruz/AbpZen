// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.advClientesChecklist;

/// <summary>
/// advClientesChecklist entity
/// </summary>
public class advClientesChecklist : FullAuditedAggregateRoot<Guid>
{
    public int? idClienteChecklist { get; set; }
    public int idCliente { get; set; }
    public int idTipoArquivo { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected advClientesChecklist()
    {
        // Required by EF Core
    }

    public advClientesChecklist(Guid id) : base(id)
    {
    }
}
