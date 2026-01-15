using System;
using System.Threading.Tasks;
using Sapienza.Dominus.EmailLog.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.EmailLog;

public interface IEmailLogAppService :
    ICrudAppService<
        EmailLogDto,
        Guid,
        EmailLogGetListInput,
        CreateUpdateEmailLogDto,
        CreateUpdateEmailLogDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetEmailLogLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
