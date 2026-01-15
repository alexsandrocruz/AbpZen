using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Transaction.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Transaction;

/// <summary>
/// Application service for Transaction entity
/// </summary>
[Authorize(TransactionPermissions.Default)]
public class TransactionAppService :
    DominusAppService,
    ITransactionAppService
{
    private readonly IRepository<Dominus.Transaction.Transaction, Guid> _repository;
    private readonly IRepository<Dominus.Client.Client, Guid> _clientRepository;
    private readonly IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> _financialCategoryRepository;

    public TransactionAppService(
        IRepository<Dominus.Transaction.Transaction, Guid> repository,
        IRepository<Dominus.Client.Client, Guid> clientRepository,
        IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> financialCategoryRepository
    )
    {
        _repository = repository;
        _clientRepository = clientRepository;
        _financialCategoryRepository = financialCategoryRepository;
    }

    /// <summary>
    /// Gets a single Transaction by Id
    /// </summary>
    public virtual async Task<TransactionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Transaction.Transaction, TransactionDto>(entity);
        if (entity.ClientId != null)
        {
            var parent = await _clientRepository.FindAsync(entity.ClientId.Value);
            dto.ClientDisplayName = parent?.Name;
        }
        if (entity.FinancialCategoryId != null)
        {
            var parent = await _financialCategoryRepository.FindAsync(entity.FinancialCategoryId.Value);
            dto.FinancialCategoryDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Transactions
    /// </summary>
    public virtual async Task<PagedResultDto<TransactionDto>> GetListAsync(TransactionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Transaction.Transaction>, List<TransactionDto>>(entities);
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
        var financialCategoryIds = entities
            .Where(x => x.FinancialCategoryId != null)
            .Select(x => x.FinancialCategoryId.Value)
            .Distinct()
            .ToList();

        if (financialCategoryIds.Any())
        {
            var parents = await _financialCategoryRepository.GetListAsync(x => financialCategoryIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.FinancialCategoryId != null))
            {
                if (parentMap.TryGetValue(dto.FinancialCategoryId.Value, out var displayName))
                {
                    dto.FinancialCategoryDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<TransactionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Transaction
    /// </summary>
    [Authorize(TransactionPermissions.Create)]
    public virtual async Task<TransactionDto> CreateAsync(CreateUpdateTransactionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateTransactionDto, Dominus.Transaction.Transaction>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Transaction.Transaction, TransactionDto>(entity);
    }

    /// <summary>
    /// Updates an existing Transaction
    /// </summary>
    [Authorize(TransactionPermissions.Update)]
    public virtual async Task<TransactionDto> UpdateAsync(Guid id, CreateUpdateTransactionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Transaction.Transaction), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Transaction.Transaction, TransactionDto>(entity);
    }

    /// <summary>
    /// Deletes a Transaction
    /// </summary>
    [Authorize(TransactionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetTransactionLookupAsync()
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
    protected virtual IQueryable<Dominus.Transaction.Transaction> ApplyFilters(IQueryable<Dominus.Transaction.Transaction> queryable, TransactionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.ClientId != null, x => x.ClientId == input.ClientId)
            .WhereIf(input.FinancialCategoryId != null, x => x.FinancialCategoryId == input.FinancialCategoryId)
            ;
    }
}
