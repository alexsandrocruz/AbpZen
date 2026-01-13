using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.autoFTP.Dtos;

[Serializable]
public class autoFTPDto : FullAuditedEntityDto<Guid>
{
    public int? id { get; set; }
    public string arquivo { get; set; }
    public bool? processado { get; set; }
    public DateTime? tsInclusao { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
