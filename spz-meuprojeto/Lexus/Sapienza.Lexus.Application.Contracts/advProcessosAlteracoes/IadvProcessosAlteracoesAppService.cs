using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessosAlteracoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessosAlteracoes;

public interface IadvProcessosAlteracoesAppService :
    ICrudAppService<
        advProcessosAlteracoesDto,
        Guid,
        advProcessosAlteracoesGetListInput,
        CreateUpdateadvProcessosAlteracoesDto,
        CreateUpdateadvProcessosAlteracoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosAlteracoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
