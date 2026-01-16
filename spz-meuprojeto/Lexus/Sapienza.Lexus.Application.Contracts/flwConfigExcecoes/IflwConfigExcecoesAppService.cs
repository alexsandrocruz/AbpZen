using System;
using System.Threading.Tasks;
using Sapienza.Lexus.flwConfigExcecoes.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.flwConfigExcecoes;

public interface IflwConfigExcecoesAppService :
    ICrudAppService<
        flwConfigExcecoesDto,
        Guid,
        flwConfigExcecoesGetListInput,
        CreateUpdateflwConfigExcecoesDto,
        CreateUpdateflwConfigExcecoesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetflwConfigExcecoesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
