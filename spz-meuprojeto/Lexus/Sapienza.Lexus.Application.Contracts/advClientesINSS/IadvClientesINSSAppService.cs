using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advClientesINSS.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advClientesINSS;

public interface IadvClientesINSSAppService :
    ICrudAppService<
        advClientesINSSDto,
        Guid,
        advClientesINSSGetListInput,
        CreateUpdateadvClientesINSSDto,
        CreateUpdateadvClientesINSSDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvClientesINSSLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
