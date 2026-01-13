using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.Lawyer.Dtos;
using Sapienza.Lexus.LawyerSpecialization.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Lawyer;

/// <summary>
/// Application service for Lawyer entity
/// </summary>
[Authorize(LawyerPermissions.Default)]
public class LawyerAppService :
    LexusAppService,
    ILawyerAppService
{
    private readonly IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> _repository;

    public LawyerAppService(
        IRepository<Sapienza.Lexus.Lawyer.Lawyer, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Lawyer by Id
    /// </summary>
    public virtual async Task<LawyerDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Lawyers
    /// </summary>
    public virtual async Task<PagedResultDto<LawyerDto>> GetListAsync(LawyerGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.Lawyer.Lawyer>, List<LawyerDto>>(entities);

        return new PagedResultDto<LawyerDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Create)]
    public virtual async Task<LawyerDto> CreateAsync(CreateUpdateLawyerDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLawyerDto, Sapienza.Lexus.Lawyer.Lawyer>(input);
        // Master-Detail: LawyerSpecialization
        if (input.LawyerSpecializations != null && input.LawyerSpecializations.Any())
        {
            foreach (var itemDto in input.LawyerSpecializations)
            {
                var item = ObjectMapper.Map<CreateUpdateLawyerSpecializationDto, Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>(itemDto);
                entity.LawyerSpecializations.Add(item);
            }
        }

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);
    }

    /// <summary>
    /// Updates an existing Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Update)]
    public virtual async Task<LawyerDto> UpdateAsync(Guid id, CreateUpdateLawyerDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.LawyerSpecializations);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.Lawyer.Lawyer), id);
        }

        ObjectMapper.Map(input, entity);
        // Master-Detail Reconciliation: LawyerSpecialization
        if (input.LawyerSpecializations != null)
        {
            // 1. Remove deleted items
            var inputIds = input.LawyerSpecializations.Select(x => x.Id).Where(x => x != Guid.Empty).ToList();
            var itemsToRemove = entity.LawyerSpecializations.Where(x => !inputIds.Contains(x.Id)).ToList();
            foreach (var item in itemsToRemove)
            {
                entity.LawyerSpecializations.Remove(item);
            }

            // 2. Add or Update
            foreach (var itemDto in input.LawyerSpecializations)
            {
                if (itemDto.Id == Guid.Empty)
                {
                    // Add new
                    var newItem = ObjectMapper.Map<CreateUpdateLawyerSpecializationDto, Sapienza.Lexus.LawyerSpecialization.LawyerSpecialization>(itemDto);
                    entity.LawyerSpecializations.Add(newItem);
                }
                else
                {
                    // Update existing
                    var existingItem = entity.LawyerSpecializations.FirstOrDefault(x => x.Id == itemDto.Id);
                    if (existingItem != null)
                    {
                        ObjectMapper.Map(itemDto, existingItem);
                    }
                }
            }
        }

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.Lawyer.Lawyer, LawyerDto>(entity);
    }

    /// <summary>
    /// Deletes a Lawyer
    /// </summary>
    [Authorize(LawyerPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLawyerLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.FullName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.Lawyer.Lawyer> ApplyFilters(IQueryable<Sapienza.Lexus.Lawyer.Lawyer> queryable, LawyerGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.FullName.Contains(input.Filter) || x.PreferredName.Contains(input.Filter))
            .WhereIf(!input.FullName.IsNullOrWhiteSpace(), x => x.FullName.Contains(input.FullName))
            .WhereIf(!input.PreferredName.IsNullOrWhiteSpace(), x => x.PreferredName.Contains(input.PreferredName))
            // ========== FK Filters ==========
            ;
    }
}
