using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finCentrosResultado.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finCentrosResultado;

public interface IfinCentrosResultadoAppService :
    ICrudAppService<
        finCentrosResultadoDto,
        Guid,
        finCentrosResultadoGetListInput,
        CreateUpdatefinCentrosResultadoDto,
        CreateUpdatefinCentrosResultadoDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinCentrosResultadoLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
