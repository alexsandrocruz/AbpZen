using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.MultiTenancy;

namespace Volo.CmsKit.Admin.PageFeedbacks;

[Serializable]
public class PageFeedbackSettingDto : EntityDto<Guid>, IMultiTenant
{
    public string EntityType { get; set; }
    public string EmailAddresses { get; set; }
    public Guid? TenantId { get; set; }
}