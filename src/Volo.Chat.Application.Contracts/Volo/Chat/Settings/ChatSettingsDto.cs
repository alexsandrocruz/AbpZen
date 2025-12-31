using System.ComponentModel.DataAnnotations;
using Volo.Chat.Messages;

namespace Volo.Chat.Settings;

public class ChatSettingsDto
{
    [Range(1, 3)]
    public ChatDeletingMessages DeletingMessages { get; set; }
    
    [Range(0, int.MaxValue)]
    public int MessageDeletionPeriod { get; set; }
    
    [Range(1, 2)]
    public ChatDeletingConversations DeletingConversations { get; set; }
}