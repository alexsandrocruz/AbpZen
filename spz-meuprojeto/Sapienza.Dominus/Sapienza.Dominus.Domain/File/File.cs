// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.File;

/// <summary>
/// File entity
/// </summary>
public class File : FullAuditedEntity<Guid>
{
    public string FileName { get; set; } = string.Empty;

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected File()
    {
        // Required by EF Core
    }

    public File(Guid id) : base(id)
    {
    }
}
