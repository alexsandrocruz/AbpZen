#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.PropostalItem.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.PropostalItem;

/// <summary>
/// Application service for PropostalItem entity
/// </summary>
[Authorize(PropostalItemPermissions.Default)]
public class PropostalItemAppService :
    LexusAppService,
    IPropostalItemAppService
{
    private readonly IRepository<Sapienza.Lexus.PropostalItem.PropostalItem, Guid> _repository;
    private readonly IRepository<Sapienza.Lexus.Proposal.Proposal, Guid> _proposalRepository;

    public PropostalItemAppService(
        IRepository<Sapienza.Lexus.PropostalItem.PropostalItem, Guid> repository,
        IRepository<Sapienza.Lexus.Proposal.Proposal, Guid> proposalRepository
    )
    {
        _repository = repository;
        _proposalRepository = proposalRepository;
    }

    /// <summary>
    /// Gets a single PropostalItem by Id
    /// </summary>
    public virtual async Task<PropostalItemDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.PropostalItem.PropostalItem, PropostalItemDto>(entity);
        if (entity.ProposalId != null)
        {
            var parent = await _proposalRepository.FindAsync(entity.ProposalId.Value);
            dto.ProposalDisplayName = parent?.Number;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of PropostalItems
    /// </summary>
    public virtual async Task<PagedResultDto<PropostalItemDto>> GetListAsync(PropostalItemGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.PropostalItem.PropostalItem>, List<PropostalItemDto>>(entities);
        var proposalIds = entities
            .Where(x => x.ProposalId != null)
            .Select(x => x.ProposalId.Value)
            .Distinct()
            .ToList();

        if (proposalIds.Any())
        {
            var parents = await _proposalRepository.GetListAsync(x => proposalIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Number);

            foreach (var dto in dtoList.Where(x => x.ProposalId != null))
            {
                if (parentMap.TryGetValue(dto.ProposalId.Value, out var displayName))
                {
                    dto.ProposalDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<PropostalItemDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new PropostalItem
    /// </summary>
    [Authorize(PropostalItemPermissions.Create)]
    public virtual async Task<PropostalItemDto> CreateAsync(CreateUpdatePropostalItemDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatePropostalItemDto, Sapienza.Lexus.PropostalItem.PropostalItem>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.PropostalItem.PropostalItem, PropostalItemDto>(entity);
    }

    /// <summary>
    /// Updates an existing PropostalItem
    /// </summary>
    [Authorize(PropostalItemPermissions.Update)]
    public virtual async Task<PropostalItemDto> UpdateAsync(Guid id, CreateUpdatePropostalItemDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.PropostalItem.PropostalItem), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.PropostalItem.PropostalItem, PropostalItemDto>(entity);
    }

    /// <summary>
    /// Deletes a PropostalItem
    /// </summary>
    [Authorize(PropostalItemPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetPropostalItemLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Desc
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.PropostalItem.PropostalItem> ApplyFilters(IQueryable<Sapienza.Lexus.PropostalItem.PropostalItem> queryable, PropostalItemGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>(x.Desc != null && x.Desc.Contains(input.Filter)))
            .WhereIf(!input.Desc.IsNullOrWhiteSpace(), x => x.Desc != null && x.Desc.Contains(input.Desc))
            .WhereIf(input.Quant != null, x => x.Quant == input.Quant)
            .WhereIf(input.UnitPrice != null, x => x.UnitPrice == input.UnitPrice)
            .WhereIf(input.Total != null, x => x.Total == input.Total)
            .WhereIf(input.ProposalId != null, x => x.ProposalId == input.ProposalId)
            // ========== FK Filters ==========
            .WhereIf(input.ProposalId != null, x => x.ProposalId == input.ProposalId)
            ;
    }
}
