using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Cursos.Permissions;
using Sapienza.Cursos.Proposal.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Cursos.Proposal;

/// <summary>
/// Application service for Proposal entity
/// </summary>
[Authorize(ProposalPermissions.Default)]
public class ProposalAppService :
    CursosAppService,
    IProposalAppService
{
    private readonly IRepository<Sapienza.Cursos.Proposal.Proposal, Guid> _repository;
    private readonly IRepository<Sapienza.Cursos.Client.Client, Guid> _clientRepository;

    public ProposalAppService(
        IRepository<Sapienza.Cursos.Proposal.Proposal, Guid> repository,
        IRepository<Sapienza.Cursos.Client.Client, Guid> clientRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
    }

    /// <summary>
    /// Gets a single Proposal by Id
    /// </summary>
    public virtual async Task<ProposalDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Cursos.Proposal.Proposal, ProposalDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Proposals
    /// </summary>
    public virtual async Task<PagedResultDto<ProposalDto>> GetListAsync(ProposalGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Cursos.Proposal.Proposal>, List<ProposalDto>>(entities);
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

        return new PagedResultDto<ProposalDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Proposal
    /// </summary>
    [Authorize(ProposalPermissions.Create)]
    public virtual async Task<ProposalDto> CreateAsync(CreateUpdateProposalDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProposalDto, Sapienza.Cursos.Proposal.Proposal>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Proposal.Proposal, ProposalDto>(entity);
    }

    /// <summary>
    /// Updates an existing Proposal
    /// </summary>
    [Authorize(ProposalPermissions.Update)]
    public virtual async Task<ProposalDto> UpdateAsync(Guid id, CreateUpdateProposalDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Cursos.Proposal.Proposal), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Cursos.Proposal.Proposal, ProposalDto>(entity);
    }

    /// <summary>
    /// Deletes a Proposal
    /// </summary>
    [Authorize(ProposalPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProposalLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Number
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Cursos.Proposal.Proposal> ApplyFilters(IQueryable<Sapienza.Cursos.Proposal.Proposal> queryable, ProposalGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Number.Contains(input.Filter) || x.Obs.Contains(input.Filter))
            .WhereIf(!input.Number.IsNullOrWhiteSpace(), x => x.Number.Contains(input.Number))
            .WhereIf(input.Date != null, x => x.Date == input.Date)
            .WhereIf(input.Validate != null, x => x.Validate == input.Validate)
            .WhereIf(!input.Obs.IsNullOrWhiteSpace(), x => x.Obs.Contains(input.Obs))
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            ;
    }
}
