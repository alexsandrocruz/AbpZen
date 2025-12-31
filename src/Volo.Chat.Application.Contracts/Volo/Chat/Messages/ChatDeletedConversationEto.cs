using System;

namespace Volo.Chat.Messages;

public class ChatDeletedConversationEto
{
    public Guid TargetUserId { get; set; }
    
    public Guid UserId { get; set; }
}