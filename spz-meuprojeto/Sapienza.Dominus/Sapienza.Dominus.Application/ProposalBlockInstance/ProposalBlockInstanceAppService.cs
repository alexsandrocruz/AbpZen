using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProposalBlockInstance.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalBlockInstance;

/// <summary>
/// Application service for ProposalBlockInstance entity
/// </summary>
[Authorize(ProposalBlockInstancePermissions.Default)]
public class ProposalBlockInstanceAppService :
    DominusAppService,
    IProposalBlockInstanceAppService
{
    private readonly IRepository<Dominus.ProposalBlockInstance.ProposalBlockInstance, Guid> _repository;
    private readonly IRepository<Dominus.Proposal.Proposal, Guid> _proposalRepository;

    public ProposalBlockInstanceAppService(
        IRepository<Dominus.ProposalBlockInstance.ProposalBlockInstance, Guid> repository,
        IRepository<Dominus.Proposal.Proposal, Guid> proposalRepository
    )
    {
        _repository = repository;
        _proposalRepository = proposalRepository;
    }

    /// <summary>
    /// Gets a single ProposalBlockInstance by Id
    /// </summary>
    public virtual async Task<ProposalBlockInstanceDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProposalBlockInstance.ProposalBlockInstance, ProposalBlockInstanceDto>(entity);
        if (entity.ProposalId != null)
        {
            var parent = await _proposalRepository.FindAsync(entity.ProposalId.Value);
            dto.ProposalDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProposalBlockInstances
    /// </summary>
    public virtual async Task<PagedResultDto<ProposalBlockInstanceDto>> GetListAsync(ProposalBlockInstanceGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProposalBlockInstance.ProposalBlockInstance>, List<ProposalBlockInstanceDto>>(entities);
        var proposalIds = entities
            .Where(x => x.ProposalId != null)
            .Select(x => x.ProposalId.Value)
            .Distinct()
            .ToList();

        if (proposalIds.Any())
        {
            var parents = await _proposalRepository.GetListAsync(x => proposalIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.ProposalId != null))
            {
                if (parentMap.TryGetValue(dto.ProposalId.Value, out var displayName))
                {
                    dto.ProposalDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<ProposalBlockInstanceDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProposalBlockInstance
    /// </summary>
    [Authorize(ProposalBlockInstancePermissions.Create)]
    public virtual async Task<ProposalBlockInstanceDto> CreateAsync(CreateUpdateProposalBlockInstanceDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProposalBlockInstanceDto, Dominus.ProposalBlockInstance.ProposalBlockInstance>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalBlockInstance.ProposalBlockInstance, ProposalBlockInstanceDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProposalBlockInstance
    /// </summary>
    [Authorize(ProposalBlockInstancePermissions.Update)]
    public virtual async Task<ProposalBlockInstanceDto> UpdateAsync(Guid id, CreateUpdateProposalBlockInstanceDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProposalBlockInstance.ProposalBlockInstance), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalBlockInstance.ProposalBlockInstance, ProposalBlockInstanceDto>(entity);
    }

    /// <summary>
    /// Deletes a ProposalBlockInstance
    /// </summary>
    [Authorize(ProposalBlockInstancePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProposalBlockInstanceLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.BlockType
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.ProposalBlockInstance.ProposalBlockInstance> ApplyFilters(IQueryable<Dominus.ProposalBlockInstance.ProposalBlockInstance> queryable, ProposalBlockInstanceGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProposalId != null, x => x.ProposalId == input.ProposalId)
            ;
    }
}
