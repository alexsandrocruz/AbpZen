using System;
using System.Threading.Tasks;
using Sapienza.Lexus.advCliLog.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Lexus.advCliLog;

public interface IadvCliLogAppService :
    ICrudAppService<
        advCliLogDto,
        Guid,
        advCliLogGetListInput,
        CreateUpdateadvCliLogDto,
        CreateUpdateadvCliLogDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetadvCliLogLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
