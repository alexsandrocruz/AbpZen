using System;
using System.Threading.Tasks;
using Sapienza.Lexus.flwAcoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.flwAcoes;

public interface IflwAcoesAppService :
    ICrudAppService<
        flwAcoesDto,
        Guid,
        flwAcoesGetListInput,
        CreateUpdateflwAcoesDto,
        CreateUpdateflwAcoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetflwAcoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
