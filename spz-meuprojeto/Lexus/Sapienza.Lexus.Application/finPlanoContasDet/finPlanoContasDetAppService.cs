using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finPlanoContasDet.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finPlanoContasDet;

/// <summary>
/// Application service for finPlanoContasDet entity
/// </summary>
[Authorize(finPlanoContasDetPermissions.Default)]
public class finPlanoContasDetAppService :
    LexusAppService,
    IfinPlanoContasDetAppService
{
    private readonly IRepository<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, Guid> _repository;

    public finPlanoContasDetAppService(
        IRepository<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finPlanoContasDet by Id
    /// </summary>
    public virtual async Task<finPlanoContasDetDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, finPlanoContasDetDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finPlanoContasDets
    /// </summary>
    public virtual async Task<PagedResultDto<finPlanoContasDetDto>> GetListAsync(finPlanoContasDetGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet>, List<finPlanoContasDetDto>>(entities);

        return new PagedResultDto<finPlanoContasDetDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finPlanoContasDet
    /// </summary>
    [Authorize(finPlanoContasDetPermissions.Create)]
    public virtual async Task<finPlanoContasDetDto> CreateAsync(CreateUpdatefinPlanoContasDetDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinPlanoContasDetDto, Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, finPlanoContasDetDto>(entity);
    }

    /// <summary>
    /// Updates an existing finPlanoContasDet
    /// </summary>
    [Authorize(finPlanoContasDetPermissions.Update)]
    public virtual async Task<finPlanoContasDetDto> UpdateAsync(Guid id, CreateUpdatefinPlanoContasDetDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet, finPlanoContasDetDto>(entity);
    }

    /// <summary>
    /// Deletes a finPlanoContasDet
    /// </summary>
    [Authorize(finPlanoContasDetPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinPlanoContasDetLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.titulo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet> ApplyFilters(IQueryable<Sapienza.Lexus.finPlanoContasDet.finPlanoContasDet> queryable, finPlanoContasDetGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idPlanoContasDet != null, x => x.idPlanoContasDet == input.idPlanoContasDet)
            .WhereIf(input.idPlanoConta != null, x => x.idPlanoConta == input.idPlanoConta)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
