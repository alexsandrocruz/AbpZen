using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadMessageTemplate.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadMessageTemplate;

/// <summary>
/// Application service for LeadMessageTemplate entity
/// </summary>
[Authorize(LeadMessageTemplatePermissions.Default)]
public class LeadMessageTemplateAppService :
    DominusAppService,
    ILeadMessageTemplateAppService
{
    private readonly IRepository<Dominus.LeadMessageTemplate.LeadMessageTemplate, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;

    public LeadMessageTemplateAppService(
        IRepository<Dominus.LeadMessageTemplate.LeadMessageTemplate, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
    }

    /// <summary>
    /// Gets a single LeadMessageTemplate by Id
    /// </summary>
    public virtual async Task<LeadMessageTemplateDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadMessageTemplate.LeadMessageTemplate, LeadMessageTemplateDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadMessageTemplates
    /// </summary>
    public virtual async Task<PagedResultDto<LeadMessageTemplateDto>> GetListAsync(LeadMessageTemplateGetListInput input)
    {
        var queryable = await _repository.GetQueryableAsync();

        // Apply filters
        queryable = ApplyFilters(queryable, input);

        // Apply default sorting (by CreationTime descending)
        queryable = queryable.OrderByDescending(e => e.CreationTime);

        // Get total count
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        // Apply paging
        queryable = queryable.PageBy(input.SkipCount, input.MaxResultCount);

        var entities = await AsyncExecuter.ToListAsync(queryable);
        var dtoList = ObjectMapper.Map<List<Dominus.LeadMessageTemplate.LeadMessageTemplate>, List<LeadMessageTemplateDto>>(entities);
        var leadWorkflowIds = entities
            .Where(x => x.LeadWorkflowId != null)
            .Select(x => x.LeadWorkflowId.Value)
            .Distinct()
            .ToList();

        if (leadWorkflowIds.Any())
        {
            var parents = await _leadWorkflowRepository.GetListAsync(x => leadWorkflowIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadWorkflowId != null))
            {
                if (parentMap.TryGetValue(dto.LeadWorkflowId.Value, out var displayName))
                {
                    dto.LeadWorkflowDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadMessageTemplateDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadMessageTemplate
    /// </summary>
    [Authorize(LeadMessageTemplatePermissions.Create)]
    public virtual async Task<LeadMessageTemplateDto> CreateAsync(CreateUpdateLeadMessageTemplateDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadMessageTemplateDto, Dominus.LeadMessageTemplate.LeadMessageTemplate>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadMessageTemplate.LeadMessageTemplate, LeadMessageTemplateDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadMessageTemplate
    /// </summary>
    [Authorize(LeadMessageTemplatePermissions.Update)]
    public virtual async Task<LeadMessageTemplateDto> UpdateAsync(Guid id, CreateUpdateLeadMessageTemplateDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadMessageTemplate.LeadMessageTemplate), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadMessageTemplate.LeadMessageTemplate, LeadMessageTemplateDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadMessageTemplate
    /// </summary>
    [Authorize(LeadMessageTemplatePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadMessageTemplateLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Name
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadMessageTemplate.LeadMessageTemplate> ApplyFilters(IQueryable<Dominus.LeadMessageTemplate.LeadMessageTemplate> queryable, LeadMessageTemplateGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            ;
    }
}
