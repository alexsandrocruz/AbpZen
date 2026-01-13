using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finPrestacaoContas.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finPrestacaoContas;

public interface IfinPrestacaoContasAppService :
    ICrudAppService<
        finPrestacaoContasDto,
        Guid,
        finPrestacaoContasGetListInput,
        CreateUpdatefinPrestacaoContasDto,
        CreateUpdatefinPrestacaoContasDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinPrestacaoContasLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
