using System;
using System.Threading.Tasks;
using Sapienza.Dominus.SmsLog.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.SmsLog;

public interface ISmsLogAppService :
    ICrudAppService<
        SmsLogDto,
        Guid,
        SmsLogGetListInput,
        CreateUpdateSmsLogDto,
        CreateUpdateSmsLogDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetSmsLogLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
