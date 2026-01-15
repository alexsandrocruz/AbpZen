using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadFormSubmission.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormSubmission;

/// <summary>
/// Application service for LeadFormSubmission entity
/// </summary>
[Authorize(LeadFormSubmissionPermissions.Default)]
public class LeadFormSubmissionAppService :
    DominusAppService,
    ILeadFormSubmissionAppService
{
    private readonly IRepository<Dominus.LeadFormSubmission.LeadFormSubmission, Guid> _repository;
    private readonly IRepository<Dominus.LeadForm.LeadForm, Guid> _leadFormRepository;

    public LeadFormSubmissionAppService(
        IRepository<Dominus.LeadFormSubmission.LeadFormSubmission, Guid> repository,
        IRepository<Dominus.LeadForm.LeadForm, Guid> leadFormRepository
    )
    {
        _repository = repository;
        _leadFormRepository = leadFormRepository;
    }

    /// <summary>
    /// Gets a single LeadFormSubmission by Id
    /// </summary>
    public virtual async Task<LeadFormSubmissionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadFormSubmission.LeadFormSubmission, LeadFormSubmissionDto>(entity);
        if (entity.LeadFormId != null)
        {
            var parent = await _leadFormRepository.FindAsync(entity.LeadFormId.Value);
            dto.LeadFormDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadFormSubmissions
    /// </summary>
    public virtual async Task<PagedResultDto<LeadFormSubmissionDto>> GetListAsync(LeadFormSubmissionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadFormSubmission.LeadFormSubmission>, List<LeadFormSubmissionDto>>(entities);
        var leadFormIds = entities
            .Where(x => x.LeadFormId != null)
            .Select(x => x.LeadFormId.Value)
            .Distinct()
            .ToList();

        if (leadFormIds.Any())
        {
            var parents = await _leadFormRepository.GetListAsync(x => leadFormIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.LeadFormId != null))
            {
                if (parentMap.TryGetValue(dto.LeadFormId.Value, out var displayName))
                {
                    dto.LeadFormDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LeadFormSubmissionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadFormSubmission
    /// </summary>
    [Authorize(LeadFormSubmissionPermissions.Create)]
    public virtual async Task<LeadFormSubmissionDto> CreateAsync(CreateUpdateLeadFormSubmissionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadFormSubmissionDto, Dominus.LeadFormSubmission.LeadFormSubmission>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadFormSubmission.LeadFormSubmission, LeadFormSubmissionDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadFormSubmission
    /// </summary>
    [Authorize(LeadFormSubmissionPermissions.Update)]
    public virtual async Task<LeadFormSubmissionDto> UpdateAsync(Guid id, CreateUpdateLeadFormSubmissionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadFormSubmission.LeadFormSubmission), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadFormSubmission.LeadFormSubmission, LeadFormSubmissionDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadFormSubmission
    /// </summary>
    [Authorize(LeadFormSubmissionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadFormSubmissionLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.IpAddress
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadFormSubmission.LeadFormSubmission> ApplyFilters(IQueryable<Dominus.LeadFormSubmission.LeadFormSubmission> queryable, LeadFormSubmissionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadFormId != null, x => x.LeadFormId == input.LeadFormId)
            ;
    }
}
