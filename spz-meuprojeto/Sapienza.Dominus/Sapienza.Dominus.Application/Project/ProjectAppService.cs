using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Project.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Project;

/// <summary>
/// Application service for Project entity
/// </summary>
[Authorize(ProjectPermissions.Default)]
public class ProjectAppService :
    DominusAppService,
    IProjectAppService
{
    private readonly IRepository<Dominus.Project.Project, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;

    public ProjectAppService(
        IRepository<Dominus.Project.Project, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single Project by Id
    /// </summary>
    public virtual async Task<ProjectDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Project.Project, ProjectDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Projects
    /// </summary>
    public virtual async Task<PagedResultDto<ProjectDto>> GetListAsync(ProjectGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Project.Project>, List<ProjectDto>>(entities);
        var clientIds = entities
            .Where(x => x.ClientId != null)
            .Select(x => x.ClientId.Value)
            .Distinct()
            .ToList();

        if (clientIds.Any())
        {
            var parents = await _clientRepository.GetListAsync(x => clientIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.ClientId != null))
            {
                if (parentMap.TryGetValue(dto.ClientId.Value, out var displayName))
                {
                    dto.ClientDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ProjectDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Project
    /// </summary>
    [Authorize(ProjectPermissions.Create)]
    public virtual async Task<ProjectDto> CreateAsync(CreateUpdateProjectDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProjectDto, Dominus.Project.Project>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Project.Project, ProjectDto>(entity);
    }

    /// <summary>
    /// Updates an existing Project
    /// </summary>
    [Authorize(ProjectPermissions.Update)]
    public virtual async Task<ProjectDto> UpdateAsync(Guid id, CreateUpdateProjectDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Project.Project), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Project.Project, ProjectDto>(entity);
    }

    /// <summary>
    /// Deletes a Project
    /// </summary>
    [Authorize(ProjectPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProjectLookupAsync()
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
    protected virtual IQueryable<Dominus.Project.Project> ApplyFilters(IQueryable<Dominus.Project.Project> queryable, ProjectGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
