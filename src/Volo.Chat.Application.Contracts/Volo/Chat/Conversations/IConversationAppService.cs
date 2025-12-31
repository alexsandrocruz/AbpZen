using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Chat.Messages;

namespace Volo.Chat.Conversations;

public interface IConversationAppService : IApplicationService
{
    Task<ChatMessageDto> SendMessageAsync(SendMessageInput input);
    
    Task DeleteMessageAsync(DeleteMessageInput input);

    Task<ChatConversationDto> GetConversationAsync(GetConversationInput input);

    Task MarkConversationAsReadAsync(MarkConversationAsReadInput input);
    
    Task DeleteConversationAsync(DeleteConversationInput input);
}
