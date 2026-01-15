using System;
using System.Threading.Tasks;
using Sapienza.Dominus.WhatsappLog.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.WhatsappLog;

public interface IWhatsappLogAppService :
    ICrudAppService<
        WhatsappLogDto,
        Guid,
        WhatsappLogGetListInput,
        CreateUpdateWhatsappLogDto,
        CreateUpdateWhatsappLogDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetWhatsappLogLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
