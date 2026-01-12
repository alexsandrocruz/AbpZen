using System;
using System.Threading.Tasks;
using LeptonXDemoApp.LeadMessage.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeptonXDemoApp.LeadMessage;

public interface ILeadMessageAppService :
    ICrudAppService<
        LeadMessageDto,
        Guid,
        LeadMessageGetListInput,
        CreateUpdateLeadMessageDto,
        CreateUpdateLeadMessageDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetLeadMessageLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
