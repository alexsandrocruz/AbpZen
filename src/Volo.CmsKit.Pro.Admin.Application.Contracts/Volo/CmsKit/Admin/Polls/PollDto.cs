using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace Volo.CmsKit.Admin.Polls;

[Serializable]
public class PollDto : ExtensibleEntityDto<Guid>, IHasCreationTime
{
    public string Question { get; set; }

    public string Code { get; set; }

    public string Widget { get; set; }

    public string Name { get; set; }

    public bool AllowMultipleVote { get; set; }

    public int VoteCount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreationTime { get; set; }
}
