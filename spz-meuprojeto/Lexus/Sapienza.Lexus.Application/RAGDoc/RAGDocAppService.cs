using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.RAGDoc.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.RAGDoc;

/// <summary>
/// Application service for RAGDoc entity
/// </summary>
[Authorize(RAGDocPermissions.Default)]
public class RAGDocAppService :
    LexusAppService,
    IRAGDocAppService
{
    private readonly IRepository<Sapienza.Lexus.RAGDoc.RAGDoc, Guid> _repository;

    public RAGDocAppService(
        IRepository<Sapienza.Lexus.RAGDoc.RAGDoc, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single RAGDoc by Id
    /// </summary>
    public virtual async Task<RAGDocDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.RAGDoc.RAGDoc, RAGDocDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of RAGDocs
    /// </summary>
    public virtual async Task<PagedResultDto<RAGDocDto>> GetListAsync(RAGDocGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.RAGDoc.RAGDoc>, List<RAGDocDto>>(entities);

        return new PagedResultDto<RAGDocDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new RAGDoc
    /// </summary>
    [Authorize(RAGDocPermissions.Create)]
    public virtual async Task<RAGDocDto> CreateAsync(CreateUpdateRAGDocDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateRAGDocDto, Sapienza.Lexus.RAGDoc.RAGDoc>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.RAGDoc.RAGDoc, RAGDocDto>(entity);
    }

    /// <summary>
    /// Updates an existing RAGDoc
    /// </summary>
    [Authorize(RAGDocPermissions.Update)]
    public virtual async Task<RAGDocDto> UpdateAsync(Guid id, CreateUpdateRAGDocDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.RAGDoc.RAGDoc), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.RAGDoc.RAGDoc, RAGDocDto>(entity);
    }

    /// <summary>
    /// Deletes a RAGDoc
    /// </summary>
    [Authorize(RAGDocPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetRAGDocLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.RAGDoc.RAGDoc> ApplyFilters(IQueryable<Sapienza.Lexus.RAGDoc.RAGDoc> queryable, RAGDocGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.Name.Contains(input.Filter))
            .WhereIf(!input.Name.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Name))
            // ========== FK Filters ==========
            ;
    }
}
