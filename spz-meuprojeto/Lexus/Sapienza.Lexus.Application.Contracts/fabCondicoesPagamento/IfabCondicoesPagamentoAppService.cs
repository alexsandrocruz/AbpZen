using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabCondicoesPagamento.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabCondicoesPagamento;

public interface IfabCondicoesPagamentoAppService :
    ICrudAppService<
        fabCondicoesPagamentoDto,
        Guid,
        fabCondicoesPagamentoGetListInput,
        CreateUpdatefabCondicoesPagamentoDto,
        CreateUpdatefabCondicoesPagamentoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabCondicoesPagamentoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
