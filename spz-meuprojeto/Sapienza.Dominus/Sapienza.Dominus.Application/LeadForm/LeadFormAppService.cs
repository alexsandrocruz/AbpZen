using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadForm.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadForm;

/// <summary>
/// Application service for LeadForm entity
/// </summary>
[Authorize(LeadFormPermissions.Default)]
public class LeadFormAppService :
    DominusAppService,
    ILeadFormAppService
{
    private readonly IRepository<Dominus.LeadForm.LeadForm, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;

    public LeadFormAppService(
        IRepository<Dominus.LeadForm.LeadForm, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
    }

    /// <summary>
    /// Gets a single LeadForm by Id
    /// </summary>
    public virtual async Task<LeadFormDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadForm.LeadForm, LeadFormDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadForms
    /// </summary>
    public virtual async Task<PagedResultDto<LeadFormDto>> GetListAsync(LeadFormGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadForm.LeadForm>, List<LeadFormDto>>(entities);
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

        return new PagedResultDto<LeadFormDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadForm
    /// </summary>
    [Authorize(LeadFormPermissions.Create)]
    public virtual async Task<LeadFormDto> CreateAsync(CreateUpdateLeadFormDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadFormDto, Dominus.LeadForm.LeadForm>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadForm.LeadForm, LeadFormDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadForm
    /// </summary>
    [Authorize(LeadFormPermissions.Update)]
    public virtual async Task<LeadFormDto> UpdateAsync(Guid id, CreateUpdateLeadFormDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadForm.LeadForm), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadForm.LeadForm, LeadFormDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadForm
    /// </summary>
    [Authorize(LeadFormPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadFormLookupAsync()
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
    protected virtual IQueryable<Dominus.LeadForm.LeadForm> ApplyFilters(IQueryable<Dominus.LeadForm.LeadForm> queryable, LeadFormGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            ;
    }
}
