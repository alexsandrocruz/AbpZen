using System;
using System.Threading.Tasks;
using Sapienza.Dominus.Conversation.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Sapienza.Dominus.Conversation;

public interface IConversationAppService :
    ICrudAppService<
        ConversationDto,
        Guid,
        ConversationGetListInput,
        CreateUpdateConversationDto,
        CreateUpdateConversationDto>
{
    Task<ListResultDto<LookupDto<Guid>>> GetConversationLookupAsync();
}

public class LookupDto<TKey>
{
    public TKey Id { get; set; }
    public string DisplayName { get; set; }
}
