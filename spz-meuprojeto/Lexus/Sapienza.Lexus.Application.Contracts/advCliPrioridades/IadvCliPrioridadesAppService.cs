using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliPrioridades.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliPrioridades;

public interface IadvCliPrioridadesAppService :
    ICrudAppService<
        advCliPrioridadesDto,
        Guid,
        advCliPrioridadesGetListInput,
        CreateUpdateadvCliPrioridadesDto,
        CreateUpdateadvCliPrioridadesDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliPrioridadesLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
