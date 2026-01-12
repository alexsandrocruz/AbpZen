using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.LawyerSpecialization.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.LawyerSpecialization;

/// <summary>
/// Application service for LawyerSpecialization entity
/// </summary>
[Authorize(LawyerSpecializationPermissions.Default)]
public class LawyerSpecializationAppService :
    CursosAppService,
    ILawyerSpecializationAppService
{
    private readonly IRepository<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, Guid> _repository;
    private readonly IRepository<Sapienza.Cursos.Lawyer.Lawyer, Guid> _lawyerRepository;
    private readonly IRepository<Sapienza.Cursos.Specialization.Specialization, Guid> _specializationRepository;

    public LawyerSpecializationAppService(
        IRepository<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, Guid> repository,
        IRepository<Sapienza.Cursos.Lawyer.Lawyer, Guid> lawyerRepository,
        IRepository<Sapienza.Cursos.Specialization.Specialization, Guid> specializationRepository
    )
    {
        _repository = repository;
        _lawyerRepository = lawyerRepository;
        _specializationRepository = specializationRepository;
    }

    /// <summary>
    /// Gets a single LawyerSpecialization by Id
    /// </summary>
    public virtual async Task<LawyerSpecializationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, LawyerSpecializationDto>(entity);
        var lawyer = await _lawyerRepository.FindAsync(entity.LawyerId);
        dto.LawyerDisplayName = lawyer?.FullName;
        var specialization = await _specializationRepository.FindAsync(entity.SpecializationId);
        dto.SpecializationDisplayName = specialization?.Name;

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of LawyerSpecializations
    /// </summary>
    public virtual async Task<PagedResultDto<LawyerSpecializationDto>> GetListAsync(LawyerSpecializationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization>, List<LawyerSpecializationDto>>(entities);
        var lawyerIds = entities
            .Select(x => x.LawyerId)
            .Distinct()
            .ToList();

        if (lawyerIds.Any())
        {
            var parents = await _lawyerRepository.GetListAsync(x => lawyerIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.FullName);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.LawyerId, out var displayName))
                {
                    dto.LawyerDisplayName = displayName;
                }
            }
        }
        var specializationIds = entities
            .Select(x => x.SpecializationId)
            .Distinct()
            .ToList();

        if (specializationIds.Any())
        {
            var parents = await _specializationRepository.GetListAsync(x => specializationIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList)
            {
                if (parentMap.TryGetValue(dto.SpecializationId, out var displayName))
                {
                    dto.SpecializationDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<LawyerSpecializationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new LawyerSpecialization
    /// </summary>
    [Authorize(LawyerSpecializationPermissions.Create)]
    public virtual async Task<LawyerSpecializationDto> CreateAsync(CreateUpdateLawyerSpecializationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateLawyerSpecializationDto, Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, LawyerSpecializationDto>(entity);
    }

    /// <summary>
    /// Updates an existing LawyerSpecialization
    /// </summary>
    [Authorize(LawyerSpecializationPermissions.Update)]
    public virtual async Task<LawyerSpecializationDto> UpdateAsync(Guid id, CreateUpdateLawyerSpecializationDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization, LawyerSpecializationDto>(entity);
    }

    /// <summary>
    /// Deletes a LawyerSpecialization
    /// </summary>
    [Authorize(LawyerSpecializationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetLawyerSpecializationLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization> ApplyFilters(IQueryable<Sapienza.Cursos.LawyerSpecialization.LawyerSpecialization> queryable, LawyerSpecializationGetListInput input)
    {
        return queryable
            .WhereIf(input.LawyerId != null, x => x.LawyerId == input.LawyerId)
            .WhereIf(input.SpecializationId != null, x => x.SpecializationId == input.SpecializationId)
            // ========== FK Filters ==========
            .WhereIf(input.LawyerId != null, x => x.LawyerId == input.LawyerId)
            .WhereIf(input.SpecializationId != null, x => x.SpecializationId == input.SpecializationId)
            ;
    }
}
