using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProposalItem.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalItem;

/// <summary>
/// Application service for ProposalItem entity
/// </summary>
[Authorize(ProposalItemPermissions.Default)]
public class ProposalItemAppService :
    DominusAppService,
    IProposalItemAppService
{
    private readonly IRepository<Dominus.ProposalItem.ProposalItem, Guid> _repository;
    private readonly IRepository<Dominus.Proposal.Proposal, Guid> _proposalRepository;

    public ProposalItemAppService(
        IRepository<Dominus.ProposalItem.ProposalItem, Guid> repository,
        IRepository<Dominus.Proposal.Proposal, Guid> proposalRepository
    )
    {
        _repository = repository;
        _proposalRepository = proposalRepository;
    }

    /// <summary>
    /// Gets a single ProposalItem by Id
    /// </summary>
    public virtual async Task<ProposalItemDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProposalItem.ProposalItem, ProposalItemDto>(entity);
        if (entity.ProposalId != null)
        {
            var parent = await _proposalRepository.FindAsync(entity.ProposalId.Value);
            dto.ProposalDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProposalItems
    /// </summary>
    public virtual async Task<PagedResultDto<ProposalItemDto>> GetListAsync(ProposalItemGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProposalItem.ProposalItem>, List<ProposalItemDto>>(entities);
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

        return new PagedResultDto<ProposalItemDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProposalItem
    /// </summary>
    [Authorize(ProposalItemPermissions.Create)]
    public virtual async Task<ProposalItemDto> CreateAsync(CreateUpdateProposalItemDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProposalItemDto, Dominus.ProposalItem.ProposalItem>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalItem.ProposalItem, ProposalItemDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProposalItem
    /// </summary>
    [Authorize(ProposalItemPermissions.Update)]
    public virtual async Task<ProposalItemDto> UpdateAsync(Guid id, CreateUpdateProposalItemDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProposalItem.ProposalItem), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalItem.ProposalItem, ProposalItemDto>(entity);
    }

    /// <summary>
    /// Deletes a ProposalItem
    /// </summary>
    [Authorize(ProposalItemPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProposalItemLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Description
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.ProposalItem.ProposalItem> ApplyFilters(IQueryable<Dominus.ProposalItem.ProposalItem> queryable, ProposalItemGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ProposalId != null, x => x.ProposalId == input.ProposalId)
            ;
    }
}
