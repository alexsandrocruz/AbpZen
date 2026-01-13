using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabMotivosAproveitamento.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabMotivosAproveitamento;

public interface IfabMotivosAproveitamentoAppService :
    ICrudAppService<
        fabMotivosAproveitamentoDto,
        Guid,
        fabMotivosAproveitamentoGetListInput,
        CreateUpdatefabMotivosAproveitamentoDto,
        CreateUpdatefabMotivosAproveitamentoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabMotivosAproveitamentoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
