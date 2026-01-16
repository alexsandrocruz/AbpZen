using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProEscritorios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProEscritorios;

public interface IadvProEscritoriosAppService :
    ICrudAppService<
        advProEscritoriosDto,
        Guid,
        advProEscritoriosGetListInput,
        CreateUpdateadvProEscritoriosDto,
        CreateUpdateadvProEscritoriosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProEscritoriosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
