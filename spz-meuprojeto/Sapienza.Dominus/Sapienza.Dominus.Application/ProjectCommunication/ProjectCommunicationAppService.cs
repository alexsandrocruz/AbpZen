using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProjectCommunication.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProjectCommunication;

/// <summary>
/// Application service for ProjectCommunication entity
/// </summary>
[Authorize(ProjectCommunicationPermissions.Default)]
public class ProjectCommunicationAppService :
    DominusAppService,
    IProjectCommunicationAppService
{
    private readonly IRepository<Dominus.ProjectCommunication.ProjectCommunication, Guid> _repository;
    private readonly IRepository<Dominus.Project.Project, Guid> _projectRepository;

    public ProjectCommunicationAppService(
        IRepository<Dominus.ProjectCommunication.ProjectCommunication, Guid> repository,
        IRepository<Dominus.Project.Project, Guid> projectRepository
    )
    {
        _repository = repository;
        _projectRepository = projectRepository;
    }

    /// <summary>
    /// Gets a single ProjectCommunication by Id
    /// </summary>
    public virtual async Task<ProjectCommunicationDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProjectCommunication.ProjectCommunication, ProjectCommunicationDto>(entity);
        if (entity.ProjectId != null)
        {
            var parent = await _projectRepository.FindAsync(entity.ProjectId.Value);
            dto.ProjectDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProjectCommunications
    /// </summary>
    public virtual async Task<PagedResultDto<ProjectCommunicationDto>> GetListAsync(ProjectCommunicationGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProjectCommunication.ProjectCommunication>, List<ProjectCommunicationDto>>(entities);
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

        return new PagedResultDto<ProjectCommunicationDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProjectCommunication
    /// </summary>
    [Authorize(ProjectCommunicationPermissions.Create)]
    public virtual async Task<ProjectCommunicationDto> CreateAsync(CreateUpdateProjectCommunicationDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProjectCommunicationDto, Dominus.ProjectCommunication.ProjectCommunication>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectCommunication.ProjectCommunication, ProjectCommunicationDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProjectCommunication
    /// </summary>
    [Authorize(ProjectCommunicationPermissions.Update)]
    public virtual async Task<ProjectCommunicationDto> UpdateAsync(Guid id, CreateUpdateProjectCommunicationDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProjectCommunication.ProjectCommunication), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProjectCommunication.ProjectCommunication, ProjectCommunicationDto>(entity);
    }

    /// <summary>
    /// Deletes a ProjectCommunication
    /// </summary>
    [Authorize(ProjectCommunicationPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProjectCommunicationLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Channel
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.ProjectCommunication.ProjectCommunication> ApplyFilters(IQueryable<Dominus.ProjectCommunication.ProjectCommunication> queryable, ProjectCommunicationGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProjectId != null, x => x.ProjectId == input.ProjectId)
            ;
    }
}
