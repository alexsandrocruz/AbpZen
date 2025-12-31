using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Admin.Polls;
[Serializable]
public class GetResultDto : EntityDto
{
    public string Question { get; set; }
    public int PollVoteCount { get; set; }
    public List<PollResultDto> PollResultDetails { get; set; }
}