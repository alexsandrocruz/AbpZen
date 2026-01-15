using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Budget.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Budget;

/// <summary>
/// Application service for Budget entity
/// </summary>
[Authorize(BudgetPermissions.Default)]
public class BudgetAppService :
    DominusAppService,
    IBudgetAppService
{
    private readonly IRepository<Dominus.Budget.Budget, Guid> _repository;
    private readonly IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> _financialCategoryRepository;

    public BudgetAppService(
        IRepository<Dominus.Budget.Budget, Guid> repository,
        IRepository<Dominus.FinancialCategory.FinancialCategory, Guid> financialCategoryRepository
    )
    {
        _repository = repository;
        _financialCategoryRepository = financialCategoryRepository;
    }

    /// <summary>
    /// Gets a single Budget by Id
    /// </summary>
    public virtual async Task<BudgetDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Budget.Budget, BudgetDto>(entity);
        if (entity.FinancialCategoryId != null)
        {
            var parent = await _financialCategoryRepository.FindAsync(entity.FinancialCategoryId.Value);
            dto.FinancialCategoryDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Budgets
    /// </summary>
    public virtual async Task<PagedResultDto<BudgetDto>> GetListAsync(BudgetGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Budget.Budget>, List<BudgetDto>>(entities);
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

        return new PagedResultDto<BudgetDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Budget
    /// </summary>
    [Authorize(BudgetPermissions.Create)]
    public virtual async Task<BudgetDto> CreateAsync(CreateUpdateBudgetDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateBudgetDto, Dominus.Budget.Budget>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Budget.Budget, BudgetDto>(entity);
    }

    /// <summary>
    /// Updates an existing Budget
    /// </summary>
    [Authorize(BudgetPermissions.Update)]
    public virtual async Task<BudgetDto> UpdateAsync(Guid id, CreateUpdateBudgetDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Budget.Budget), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Budget.Budget, BudgetDto>(entity);
    }

    /// <summary>
    /// Deletes a Budget
    /// </summary>
    [Authorize(BudgetPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBudgetLookupAsync()
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
    protected virtual IQueryable<Dominus.Budget.Budget> ApplyFilters(IQueryable<Dominus.Budget.Budget> queryable, BudgetGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.FinancialCategoryId != null, x => x.FinancialCategoryId == input.FinancialCategoryId)
            ;
    }
}
