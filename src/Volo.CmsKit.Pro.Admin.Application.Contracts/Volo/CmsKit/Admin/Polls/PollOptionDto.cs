using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Polls;

[Serializable]
public class PollOptionDto : EntityDto<Guid>
{
    public string Text { get; set; }
    
    public int Order { get; set; }
    public int VoteCount { get; set; }
}
