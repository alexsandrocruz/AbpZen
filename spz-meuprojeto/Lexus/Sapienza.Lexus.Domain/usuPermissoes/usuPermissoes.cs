// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.usuPermissoes;

/// <summary>
/// usuPermissoes entity
/// </summary>
public class usuPermissoes : FullAuditedAggregateRoot<Guid>
{
    public int? idUsuarioPermissao { get; set; }
    public int? idUsuario { get; set; }
    public int? idPermissao { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected usuPermissoes()
    {
        // Required by EF Core
    }

    public usuPermissoes(Guid id) : base(id)
    {
    }
}
