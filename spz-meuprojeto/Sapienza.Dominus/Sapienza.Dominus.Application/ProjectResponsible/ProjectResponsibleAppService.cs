using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProjectResponsible.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectResponsible;

/// <summary>
/// Application service for ProjectResponsible entity
/// </summary>
[Authorize(ProjectResponsiblePermissions.Default)]
public class ProjectResponsibleAppService :
    DominusAppService,
    IProjectResponsibleAppService
{
    private readonly IRepository<Dominus.ProjectResponsible.ProjectResponsible, Guid> _repository;
    private readonly IRepository<Dominus.Project.Project, Guid> _projectRepository;

    public ProjectResponsibleAppService(
        IRepository<Dominus.ProjectResponsible.ProjectResponsible, Guid> repository,
        IRepository<Dominus.Project.Project, Guid> projectRepository
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Gets a single ProjectResponsible by Id
    /// </summary>
    public virtual async Task<ProjectResponsibleDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProjectResponsible.ProjectResponsible, ProjectResponsibleDto>(entity);
        if (entity.ProjectId != null)
        {
            var parent = await _projectRepository.FindAsync(entity.ProjectId.Value);
            dto.ProjectDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProjectResponsibles
    /// </summary>
    public virtual async Task<PagedResultDto<ProjectResponsibleDto>> GetListAsync(ProjectResponsibleGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProjectResponsible.ProjectResponsible>, List<ProjectResponsibleDto>>(entities);
        var projectIds = entities
            .Where(x => x.ProjectId != null)
            .Select(x => x.ProjectId.Value)
            .Distinct()
            .ToList();

        if (projectIds.Any())
        {
            var parents = await _projectRepository.GetListAsync(x => projectIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.ProjectId != null))
            {
                if (parentMap.TryGetValue(dto.ProjectId.Value, out var displayName))
                {
                    dto.ProjectDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ProjectResponsibleDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProjectResponsible
    /// </summary>
    [Authorize(ProjectResponsiblePermissions.Create)]
    public virtual async Task<ProjectResponsibleDto> CreateAsync(CreateUpdateProjectResponsibleDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProjectResponsibleDto, Dominus.ProjectResponsible.ProjectResponsible>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectResponsible.ProjectResponsible, ProjectResponsibleDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProjectResponsible
    /// </summary>
    [Authorize(ProjectResponsiblePermissions.Update)]
    public virtual async Task<ProjectResponsibleDto> UpdateAsync(Guid id, CreateUpdateProjectResponsibleDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProjectResponsible.ProjectResponsible), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectResponsible.ProjectResponsible, ProjectResponsibleDto>(entity);
    }

    /// <summary>
    /// Deletes a ProjectResponsible
    /// </summary>
    [Authorize(ProjectResponsiblePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProjectResponsibleLookupAsync()
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
    protected virtual IQueryable<Dominus.ProjectResponsible.ProjectResponsible> ApplyFilters(IQueryable<Dominus.ProjectResponsible.ProjectResponsible> queryable, ProjectResponsibleGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProjectId != null, x => x.ProjectId == input.ProjectId)
            ;
    }
}
