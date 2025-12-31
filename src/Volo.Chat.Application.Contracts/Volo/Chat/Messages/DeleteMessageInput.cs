using System;

namespace Volo.Chat.Messages;

public class DeleteMessageInput
{
    public Guid TargetUserId { get; set; }
    
    public Guid MessageId { get; set; }
}