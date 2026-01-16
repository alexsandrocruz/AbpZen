using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesINSSStatus.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesINSSStatus;

public interface IadvClientesINSSStatusAppService :
    ICrudAppService<
        advClientesINSSStatusDto,
        Guid,
        advClientesINSSStatusGetListInput,
        CreateUpdateadvClientesINSSStatusDto,
        CreateUpdateadvClientesINSSStatusDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesINSSStatusLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
