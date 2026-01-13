using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.autoFTP.Dtos;

[Serializable]
public class CreateUpdateautoFTPDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? id { get; set; }
    public string arquivo { get; set; }
    public bool? processado { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
