using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.LeadFormField.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.LeadFormField;

/// <summary>
/// Application service for LeadFormField entity
/// </summary>
[Authorize(LeadFormFieldPermissions.Default)]
public class LeadFormFieldAppService :
    DominusAppService,
    ILeadFormFieldAppService
{
    private readonly IRepository<Dominus.LeadFormField.LeadFormField, Guid> _repository;
    private readonly IRepository<Dominus.LeadForm.LeadForm, Guid> _leadFormRepository;

    public LeadFormFieldAppService(
        IRepository<Dominus.LeadFormField.LeadFormField, Guid> repository,
        IRepository<Dominus.LeadForm.LeadForm, Guid> leadFormRepository
    )
    {
        _repository = repository;
        _leadFormRepository = leadFormRepository;
    }

    /// <summary>
    /// Gets a single LeadFormField by Id
    /// </summary>
    public virtual async Task<LeadFormFieldDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.LeadFormField.LeadFormField, LeadFormFieldDto>(entity);
        if (entity.LeadFormId != null)
        {
            var parent = await _leadFormRepository.FindAsync(entity.LeadFormId.Value);
            dto.LeadFormDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LeadFormFields
    /// </summary>
    public virtual async Task<PagedResultDto<LeadFormFieldDto>> GetListAsync(LeadFormFieldGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.LeadFormField.LeadFormField>, List<LeadFormFieldDto>>(entities);
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

        return new PagedResultDto<LeadFormFieldDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LeadFormField
    /// </summary>
    [Authorize(LeadFormFieldPermissions.Create)]
    public virtual async Task<LeadFormFieldDto> CreateAsync(CreateUpdateLeadFormFieldDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLeadFormFieldDto, Dominus.LeadFormField.LeadFormField>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadFormField.LeadFormField, LeadFormFieldDto>(entity);
    }

    /// <summary>
    /// Updates an existing LeadFormField
    /// </summary>
    [Authorize(LeadFormFieldPermissions.Update)]
    public virtual async Task<LeadFormFieldDto> UpdateAsync(Guid id, CreateUpdateLeadFormFieldDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.LeadFormField.LeadFormField), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.LeadFormField.LeadFormField, LeadFormFieldDto>(entity);
    }

    /// <summary>
    /// Deletes a LeadFormField
    /// </summary>
    [Authorize(LeadFormFieldPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLeadFormFieldLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Label
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.LeadFormField.LeadFormField> ApplyFilters(IQueryable<Dominus.LeadFormField.LeadFormField> queryable, LeadFormFieldGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.LeadFormId != null, x => x.LeadFormId == input.LeadFormId)
            ;
    }
}
