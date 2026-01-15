using System;
using System.Threading.Tasks;
using Sapienza.Dominus.AiChatSession.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.AiChatSession;

public interface IAiChatSessionAppService :
    ICrudAppService<
        AiChatSessionDto,
        Guid,
        AiChatSessionGetListInput,
        CreateUpdateAiChatSessionDto,
        CreateUpdateAiChatSessionDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetAiChatSessionLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
