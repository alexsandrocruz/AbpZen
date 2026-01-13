using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliSituacoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliSituacoes;

public interface IadvCliSituacoesAppService :
    ICrudAppService<
        advCliSituacoesDto,
        Guid,
        advCliSituacoesGetListInput,
        CreateUpdateadvCliSituacoesDto,
        CreateUpdateadvCliSituacoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliSituacoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
