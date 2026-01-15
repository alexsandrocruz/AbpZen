using System;
using System.Threading.Tasks;
using Sapienza.Dominus.LeadScheduledMessage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.LeadScheduledMessage;

public interface ILeadScheduledMessageAppService :
    ICrudAppService<
        LeadScheduledMessageDto,
        Guid,
        LeadScheduledMessageGetListInput,
        CreateUpdateLeadScheduledMessageDto,
        CreateUpdateLeadScheduledMessageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadScheduledMessageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
