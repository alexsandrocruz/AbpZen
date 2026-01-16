using System;
using System.Threading.Tasks;
using Sapienza.Lexus.fabCidades.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.fabCidades;

public interface IfabCidadesAppService :
    ICrudAppService<
        fabCidadesDto,
        Guid,
        fabCidadesGetListInput,
        CreateUpdatefabCidadesDto,
        CreateUpdatefabCidadesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfabCidadesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
