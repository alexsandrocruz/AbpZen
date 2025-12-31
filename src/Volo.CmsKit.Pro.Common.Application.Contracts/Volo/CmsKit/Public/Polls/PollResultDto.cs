using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Polls;

[Serializable]
public class PollResultDto : EntityDto
{
    public bool IsSelectedForCurrentUser { get; set; }
    public string Text { get; set; }
    public int VoteCount { get; set; }
}
