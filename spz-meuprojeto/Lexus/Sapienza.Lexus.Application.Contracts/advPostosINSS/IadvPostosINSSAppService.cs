using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advPostosINSS.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advPostosINSS;

public interface IadvPostosINSSAppService :
    ICrudAppService<
        advPostosINSSDto,
        Guid,
        advPostosINSSGetListInput,
        CreateUpdateadvPostosINSSDto,
        CreateUpdateadvPostosINSSDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvPostosINSSLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
