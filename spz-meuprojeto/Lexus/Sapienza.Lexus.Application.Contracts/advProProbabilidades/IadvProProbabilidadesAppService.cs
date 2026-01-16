using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProProbabilidades.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProProbabilidades;

public interface IadvProProbabilidadesAppService :
    ICrudAppService<
        advProProbabilidadesDto,
        Guid,
        advProProbabilidadesGetListInput,
        CreateUpdateadvProProbabilidadesDto,
        CreateUpdateadvProProbabilidadesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProProbabilidadesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
