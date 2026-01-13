using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.EventoZen.ArtistSpecialty.Dtos;

[Serializable]
public class CreateUpdateArtistSpecialtyDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? ArtistId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
