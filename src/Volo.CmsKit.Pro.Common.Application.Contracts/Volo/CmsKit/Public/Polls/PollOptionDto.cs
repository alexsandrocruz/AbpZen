using System;
using Volo.Abp.Application.Dtos;

namespace Volo.CmsKit.Public.Polls;

[Serializable]
public class PollOptionDto : EntityDto<Guid>
{
    public string Text { get; set; }
}