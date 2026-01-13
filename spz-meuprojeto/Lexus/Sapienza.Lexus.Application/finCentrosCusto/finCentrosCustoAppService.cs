using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finCentrosCusto.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finCentrosCusto;

/// <summary>
/// Application service for finCentrosCusto entity
/// </summary>
[Authorize(finCentrosCustoPermissions.Default)]
public class finCentrosCustoAppService :
    LexusAppService,
    IfinCentrosCustoAppService
{
    private readonly IRepository<Sapienza.Lexus.finCentrosCusto.finCentrosCusto, Guid> _repository;

    public finCentrosCustoAppService(
        IRepository<Sapienza.Lexus.finCentrosCusto.finCentrosCusto, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finCentrosCusto by Id
    /// </summary>
    public virtual async Task<finCentrosCustoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finCentrosCusto.finCentrosCusto, finCentrosCustoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finCentrosCustos
    /// </summary>
    public virtual async Task<PagedResultDto<finCentrosCustoDto>> GetListAsync(finCentrosCustoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finCentrosCusto.finCentrosCusto>, List<finCentrosCustoDto>>(entities);

        return new PagedResultDto<finCentrosCustoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finCentrosCusto
    /// </summary>
    [Authorize(finCentrosCustoPermissions.Create)]
    public virtual async Task<finCentrosCustoDto> CreateAsync(CreateUpdatefinCentrosCustoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinCentrosCustoDto, Sapienza.Lexus.finCentrosCusto.finCentrosCusto>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finCentrosCusto.finCentrosCusto, finCentrosCustoDto>(entity);
    }

    /// <summary>
    /// Updates an existing finCentrosCusto
    /// </summary>
    [Authorize(finCentrosCustoPermissions.Update)]
    public virtual async Task<finCentrosCustoDto> UpdateAsync(Guid id, CreateUpdatefinCentrosCustoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finCentrosCusto.finCentrosCusto), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finCentrosCusto.finCentrosCusto, finCentrosCustoDto>(entity);
    }

    /// <summary>
    /// Deletes a finCentrosCusto
    /// </summary>
    [Authorize(finCentrosCustoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinCentrosCustoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finCentrosCusto.finCentrosCusto> ApplyFilters(IQueryable<Sapienza.Lexus.finCentrosCusto.finCentrosCusto> queryable, finCentrosCustoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idCentroCusto != null, x => x.idCentroCusto == input.idCentroCusto)
            .WhereIf(input.idUnidade != null, x => x.idUnidade == input.idUnidade)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.padrao != null, x => x.padrao == input.padrao)
            .WhereIf(input.porcentagemRateio != null, x => x.porcentagemRateio == input.porcentagemRateio)
            // ========== FK Filters ==========
            ;
    }
}
