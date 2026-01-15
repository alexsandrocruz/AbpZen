using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadLandingPage.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadLandingPage;

/// <summary>
/// Application service for LeadLandingPage entity
/// </summary>
[Authorize(LeadLandingPagePermissions.Default)]
public class LeadLandingPageAppService :
    DominusAppService,
    ILeadLandingPageAppService
{
    private readonly IRepository<Dominus.LeadLandingPage.LeadLandingPage, Guid> _repository;
    private readonly IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> _leadWorkflowRepository;

    public LeadLandingPageAppService(
        IRepository<Dominus.LeadLandingPage.LeadLandingPage, Guid> repository,
        IRepository<Dominus.LeadWorkflow.LeadWorkflow, Guid> leadWorkflowRepository
    )
    {
        _repository = repository;
        _leadWorkflowRepository = leadWorkflowRepository;
    }

    /// <summary>
    /// Gets a single LeadLandingPage by Id
    /// </summary>
    public virtual async Task<LeadLandingPageDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadLandingPage.LeadLandingPage, LeadLandingPageDto>(entity);
        if (entity.LeadWorkflowId != null)
        {
            var parent = await _leadWorkflowRepository.FindAsync(entity.LeadWorkflowId.Value);
            dto.LeadWorkflowDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadLandingPages
    /// </summary>
    public virtual async Task<PagedResultDto<LeadLandingPageDto>> GetListAsync(LeadLandingPageGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadLandingPage.LeadLandingPage>, List<LeadLandingPageDto>>(entities);
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

        return new PagedResultDto<LeadLandingPageDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadLandingPage
    /// </summary>
    [Authorize(LeadLandingPagePermissions.Create)]
    public virtual async Task<LeadLandingPageDto> CreateAsync(CreateUpdateLeadLandingPageDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadLandingPageDto, Dominus.LeadLandingPage.LeadLandingPage>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadLandingPage.LeadLandingPage, LeadLandingPageDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadLandingPage
    /// </summary>
    [Authorize(LeadLandingPagePermissions.Update)]
    public virtual async Task<LeadLandingPageDto> UpdateAsync(Guid id, CreateUpdateLeadLandingPageDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadLandingPage.LeadLandingPage), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadLandingPage.LeadLandingPage, LeadLandingPageDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadLandingPage
    /// </summary>
    [Authorize(LeadLandingPagePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadLandingPageLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadLandingPage.LeadLandingPage> ApplyFilters(IQueryable<Dominus.LeadLandingPage.LeadLandingPage> queryable, LeadLandingPageGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadWorkflowId != null, x => x.LeadWorkflowId == input.LeadWorkflowId)
            ;
    }
}
