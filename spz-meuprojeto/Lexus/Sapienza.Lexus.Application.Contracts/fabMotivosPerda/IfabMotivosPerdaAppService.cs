using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabMotivosPerda.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabMotivosPerda;

public interface IfabMotivosPerdaAppService :
    ICrudAppService<
        fabMotivosPerdaDto,
        Guid,
        fabMotivosPerdaGetListInput,
        CreateUpdatefabMotivosPerdaDto,
        CreateUpdatefabMotivosPerdaDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabMotivosPerdaLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
