using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advProcessosHonorarios;

public interface IadvProcessosHonorariosAppService :
    ICrudAppService<
        advProcessosHonorariosDto,
        Guid,
        advProcessosHonorariosGetListInput,
        CreateUpdateadvProcessosHonorariosDto,
        CreateUpdateadvProcessosHonorariosDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosHonorariosLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
