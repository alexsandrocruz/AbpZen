using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.Specialization.Dtos;
using Sapienza.Lexus.LawyerSpecialization.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.Specialization;

/// <summary>
/// Application service for Specialization entity
/// </summary>
[Authorize(SpecializationPermissions.Default)]
public class SpecializationAppService :
    LexusAppService,
    ISpecializationAppService
{
    private readonly IRepository<Sapienza.Lexus.Specialization.Specialization, Guid> _repository;

    public SpecializationAppService(
        IRepository<Sapienza.Lexus.Specialization.Specialization, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Specialization by Id
    /// </summary>
    public virtual async Task<SpecializationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.Specialization.Specialization, SpecializationDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Specializations
    /// </summary>
    public virtual async Task<PagedResultDto<SpecializationDto>> GetListAsync(SpecializationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.Specialization.Specialization>, List<SpecializationDto>>(entities);

        return new PagedResultDto<SpecializationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Specialization
    /// </summary>
    [Authorize(SpecializationPermissions.Create)]
    public virtual async Task<SpecializationDto> CreateAsync(CreateUpdateSpecializationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateSpecializationDto, Sapienza.Lexus.Specialization.Specialization>(input);
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

        return ObjectMapper.Map<Sapienza.Lexus.Specialization.Specialization, SpecializationDto>(entity);
    }

    /// <summary>
    /// Updates an existing Specialization
    /// </summary>
    [Authorize(SpecializationPermissions.Update)]
    public virtual async Task<SpecializationDto> UpdateAsync(Guid id, CreateUpdateSpecializationDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync(x => x.LawyerSpecializations);
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.Specialization.Specialization), id);
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

        return ObjectMapper.Map<Sapienza.Lexus.Specialization.Specialization, SpecializationDto>(entity);
    }

    /// <summary>
    /// Deletes a Specialization
    /// </summary>
    [Authorize(SpecializationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetSpecializationLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.Specialization.Specialization> ApplyFilters(IQueryable<Sapienza.Lexus.Specialization.Specialization> queryable, SpecializationGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            // ========== FK Filters ==========
            ;
    }
}
