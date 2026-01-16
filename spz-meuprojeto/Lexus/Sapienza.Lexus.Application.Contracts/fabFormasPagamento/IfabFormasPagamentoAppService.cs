using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabFormasPagamento.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabFormasPagamento;

public interface IfabFormasPagamentoAppService :
    ICrudAppService<
        fabFormasPagamentoDto,
        Guid,
        fabFormasPagamentoGetListInput,
        CreateUpdatefabFormasPagamentoDto,
        CreateUpdatefabFormasPagamentoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabFormasPagamentoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
