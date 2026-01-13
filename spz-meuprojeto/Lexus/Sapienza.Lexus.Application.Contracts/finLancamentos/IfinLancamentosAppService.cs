using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finLancamentos.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finLancamentos;

public interface IfinLancamentosAppService :
    ICrudAppService<
        finLancamentosDto,
        Guid,
        finLancamentosGetListInput,
        CreateUpdatefinLancamentosDto,
        CreateUpdatefinLancamentosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinLancamentosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
