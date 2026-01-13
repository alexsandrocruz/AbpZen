using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPreMotivosPerda.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPreMotivosPerda;

public interface IadvPreMotivosPerdaAppService :
    ICrudAppService<
        advPreMotivosPerdaDto,
        Guid,
        advPreMotivosPerdaGetListInput,
        CreateUpdateadvPreMotivosPerdaDto,
        CreateUpdateadvPreMotivosPerdaDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPreMotivosPerdaLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
