using System;
using System.Threading.Tasks;
using Sapienza.Lexus.logAcoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.logAcoes;

public interface IlogAcoesAppService :
    ICrudAppService<
        logAcoesDto,
        Guid,
        logAcoesGetListInput,
        CreateUpdatelogAcoesDto,
        CreateUpdatelogAcoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetlogAcoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
