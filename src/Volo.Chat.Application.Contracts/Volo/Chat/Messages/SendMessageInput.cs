using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Volo.Chat.Messages;

public class SendMessageInput
{
    public Guid TargetUserId { get; set; }

    [Required]
    [DynamicStringLength(typeof(ChatMessageConsts),nameof(ChatMessageConsts.MaxTextLength), nameof(ChatMessageConsts.MinTextLength))]
    public string Message { get; set; }
}
