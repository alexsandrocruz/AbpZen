using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.TransactionAttachment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.TransactionAttachment;

/// <summary>
/// Application service for TransactionAttachment entity
/// </summary>
[Authorize(TransactionAttachmentPermissions.Default)]
public class TransactionAttachmentAppService :
    DominusAppService,
    ITransactionAttachmentAppService
{
    private readonly IRepository<Dominus.TransactionAttachment.TransactionAttachment, Guid> _repository;
    private readonly IRepository<Dominus.Transaction.Transaction, Guid> _transactionRepository;

    public TransactionAttachmentAppService(
        IRepository<Dominus.TransactionAttachment.TransactionAttachment, Guid> repository,
        IRepository<Dominus.Transaction.Transaction, Guid> transactionRepository
    )
    {
        _repository = repository;
        _transactionRepository = transactionRepository;
    }

    /// <summary>
    /// Gets a single TransactionAttachment by Id
    /// </summary>
    public virtual async Task<TransactionAttachmentDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.TransactionAttachment.TransactionAttachment, TransactionAttachmentDto>(entity);
        if (entity.TransactionId != null)
        {
            var parent = await _transactionRepository.FindAsync(entity.TransactionId.Value);
            dto.TransactionDisplayName = parent?.Description;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of TransactionAttachments
    /// </summary>
    public virtual async Task<PagedResultDto<TransactionAttachmentDto>> GetListAsync(TransactionAttachmentGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.TransactionAttachment.TransactionAttachment>, List<TransactionAttachmentDto>>(entities);
        var transactionIds = entities
            .Where(x => x.TransactionId != null)
            .Select(x => x.TransactionId.Value)
            .Distinct()
            .ToList();

        if (transactionIds.Any())
        {
            var parents = await _transactionRepository.GetListAsync(x => transactionIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Description);

            foreach (var dto in dtoList.Where(x => x.TransactionId != null))
            {
                if (parentMap.TryGetValue(dto.TransactionId.Value, out var displayName))
                {
                    dto.TransactionDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<TransactionAttachmentDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new TransactionAttachment
    /// </summary>
    [Authorize(TransactionAttachmentPermissions.Create)]
    public virtual async Task<TransactionAttachmentDto> CreateAsync(CreateUpdateTransactionAttachmentDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTransactionAttachmentDto, Dominus.TransactionAttachment.TransactionAttachment>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TransactionAttachment.TransactionAttachment, TransactionAttachmentDto>(entity);
    }

    /// <summary>
    /// Updates an existing TransactionAttachment
    /// </summary>
    [Authorize(TransactionAttachmentPermissions.Update)]
    public virtual async Task<TransactionAttachmentDto> UpdateAsync(Guid id, CreateUpdateTransactionAttachmentDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.TransactionAttachment.TransactionAttachment), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.TransactionAttachment.TransactionAttachment, TransactionAttachmentDto>(entity);
    }

    /// <summary>
    /// Deletes a TransactionAttachment
    /// </summary>
    [Authorize(TransactionAttachmentPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTransactionAttachmentLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.FileName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.TransactionAttachment.TransactionAttachment> ApplyFilters(IQueryable<Dominus.TransactionAttachment.TransactionAttachment> queryable, TransactionAttachmentGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.TransactionId != null, x => x.TransactionId == input.TransactionId)
            ;
    }
}
