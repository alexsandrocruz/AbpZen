using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabFormasRecebimento;

public interface IfabFormasRecebimentoAppService :
    ICrudAppService<
        fabFormasRecebimentoDto,
        Guid,
        fabFormasRecebimentoGetListInput,
        CreateUpdatefabFormasRecebimentoDto,
        CreateUpdatefabFormasRecebimentoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabFormasRecebimentoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
