using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.ProposalTemplateBlock.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.ProposalTemplateBlock;

/// <summary>
/// Application service for ProposalTemplateBlock entity
/// </summary>
[Authorize(ProposalTemplateBlockPermissions.Default)]
public class ProposalTemplateBlockAppService :
    DominusAppService,
    IProposalTemplateBlockAppService
{
    private readonly IRepository<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, Guid> _repository;

    public ProposalTemplateBlockAppService(
        IRepository<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single ProposalTemplateBlock by Id
    /// </summary>
    public virtual async Task<ProposalTemplateBlockDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, ProposalTemplateBlockDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of ProposalTemplateBlocks
    /// </summary>
    public virtual async Task<PagedResultDto<ProposalTemplateBlockDto>> GetListAsync(ProposalTemplateBlockGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.ProposalTemplateBlock.ProposalTemplateBlock>, List<ProposalTemplateBlockDto>>(entities);

        return new PagedResultDto<ProposalTemplateBlockDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new ProposalTemplateBlock
    /// </summary>
    [Authorize(ProposalTemplateBlockPermissions.Create)]
    public virtual async Task<ProposalTemplateBlockDto> CreateAsync(CreateUpdateProposalTemplateBlockDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateProposalTemplateBlockDto, Dominus.ProposalTemplateBlock.ProposalTemplateBlock>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, ProposalTemplateBlockDto>(entity);
    }

    /// <summary>
    /// Updates an existing ProposalTemplateBlock
    /// </summary>
    [Authorize(ProposalTemplateBlockPermissions.Update)]
    public virtual async Task<ProposalTemplateBlockDto> UpdateAsync(Guid id, CreateUpdateProposalTemplateBlockDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.ProposalTemplateBlock.ProposalTemplateBlock), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.ProposalTemplateBlock.ProposalTemplateBlock, ProposalTemplateBlockDto>(entity);
    }

    /// <summary>
    /// Deletes a ProposalTemplateBlock
    /// </summary>
    [Authorize(ProposalTemplateBlockPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetProposalTemplateBlockLookupAsync()
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
    protected virtual IQueryable<Dominus.ProposalTemplateBlock.ProposalTemplateBlock> ApplyFilters(IQueryable<Dominus.ProposalTemplateBlock.ProposalTemplateBlock> queryable, ProposalTemplateBlockGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
