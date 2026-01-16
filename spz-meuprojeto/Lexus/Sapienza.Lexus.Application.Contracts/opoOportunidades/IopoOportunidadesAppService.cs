using System;
using System.Threading.Tasks;
using Sapienza.Lexus.opoOportunidades.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.opoOportunidades;

public interface IopoOportunidadesAppService :
    ICrudAppService<
        opoOportunidadesDto,
        Guid,
        opoOportunidadesGetListInput,
        CreateUpdateopoOportunidadesDto,
        CreateUpdateopoOportunidadesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetopoOportunidadesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
