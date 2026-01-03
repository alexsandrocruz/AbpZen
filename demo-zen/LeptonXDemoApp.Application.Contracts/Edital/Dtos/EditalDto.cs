using System;
using Volo.Abp.Application.Dtos;

namespace LeptonXDemoApp.Edital.Dtos;

[Serializable]
public class EditalDto : FullAuditedEntityDto<Guid>
{
    public string Objeto { get; set; }
    public DateTime? Data { get; set; }
    public decimal? Valor { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
