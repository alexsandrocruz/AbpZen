using System;
using System.Threading.Tasks;
using Sapienza.Lexus.opoSituacoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.opoSituacoes;

public interface IopoSituacoesAppService :
    ICrudAppService<
        opoSituacoesDto,
        Guid,
        opoSituacoesGetListInput,
        CreateUpdateopoSituacoesDto,
        CreateUpdateopoSituacoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetopoSituacoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
