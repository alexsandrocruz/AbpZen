using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.LanguageManagement.Dto;

public class LanguageDto : ExtensibleCreationAuditedEntityDto<Guid>, IHasConcurrencyStamp
{
    public string CultureName { get; set; }

    public string UiCultureName { get; set; }

    public string DisplayName { get; set; }

    public bool IsEnabled { get; set; }

    public bool IsDefaultLanguage { get; set; }

    public string ConcurrencyStamp { get; set; }
}
