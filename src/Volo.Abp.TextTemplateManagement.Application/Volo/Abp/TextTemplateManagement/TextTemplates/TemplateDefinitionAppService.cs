using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Features;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplateManagement.Authorization;
using Volo.Abp.TextTemplating;
using System.Linq.Dynamic.Core;

namespace Volo.Abp.TextTemplateManagement.TextTemplates;

[RequiresFeature(TextManagementFeatures.Enable)]
[Authorize(TextTemplateManagementPermissions.TextTemplates.Default)]
public class TemplateDefinitionAppService : TextTemplateManagementAppServiceBase, ITemplateDefinitionAppService
{
    private readonly ITemplateDefinitionManager _templateDefinitionManager;

    public TemplateDefinitionAppService(ITemplateDefinitionManager templateDefinitionManager)
    {
        _templateDefinitionManager = templateDefinitionManager;
    }

    public virtual async Task<PagedResultDto<TemplateDefinitionDto>> GetListAsync(GetTemplateDefinitionListInput input)
    {
        var templateDefinitions = await _templateDefinitionManager.GetAllAsync();

        var dtos = new List<TemplateDefinitionDto>();

        foreach (var templateDefinition in templateDefinitions)
        {
            var dto = ObjectMapper.Map<TemplateDefinition, TemplateDefinitionDto>(templateDefinition);
            dto.DisplayName = templateDefinition.GetLocalizedDisplayName(StringLocalizerFactory);
            dtos.Add(dto);
        }

        if (!string.IsNullOrWhiteSpace(input.FilterText))
        {
            input.FilterText = input.FilterText.ToUpper();

            dtos = dtos.Where(
                x =>
                    (x.DisplayName != null && x.DisplayName.ToUpper().Contains(input.FilterText)) ||
                    x.Name.ToUpper().Contains(input.FilterText) ||
                    (x.DefaultCultureName != null && x.DefaultCultureName.Contains(input.FilterText))
            ).ToList();
        }

        dtos = dtos.AsQueryable().OrderBy(input.Sorting.IsNullOrEmpty() ? $"{nameof(TemplateDefinition.Name)} desc" : input.Sorting).ToList();

        var totalCount = dtos.Count;

        dtos = dtos
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

        return new PagedResultDto<TemplateDefinitionDto>(totalCount, dtos);
    }

    public virtual async Task<TemplateDefinitionDto> GetAsync(string name)
    {
        var definition = await _templateDefinitionManager.GetAsync(name);
        var definitionDto = ObjectMapper.Map<TemplateDefinition, TemplateDefinitionDto>(definition);
        definitionDto.DisplayName = definition.GetLocalizedDisplayName(StringLocalizerFactory);

        return definitionDto;
    }
}
