using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Polls;

[Serializable]
public class PollResultDto : EntityDto
{
    public string Text { get; set; }
    public int VoteCount { get; set; }
}
