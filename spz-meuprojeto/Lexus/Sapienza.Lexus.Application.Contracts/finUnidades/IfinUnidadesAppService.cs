using System;
using System.Threading.Tasks;
using Sapienza.Lexus.finUnidades.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.finUnidades;

public interface IfinUnidadesAppService :
    ICrudAppService<
        finUnidadesDto,
        Guid,
        finUnidadesGetListInput,
        CreateUpdatefinUnidadesDto,
        CreateUpdatefinUnidadesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetfinUnidadesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
