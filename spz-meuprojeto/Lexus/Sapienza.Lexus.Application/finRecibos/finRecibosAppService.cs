using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finRecibos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finRecibos;

/// <summary>
/// Application service for finRecibos entity
/// </summary>
[Authorize(finRecibosPermissions.Default)]
public class finRecibosAppService :
    LexusAppService,
    IfinRecibosAppService
{
    private readonly IRepository<Sapienza.Lexus.finRecibos.finRecibos, Guid> _repository;

    public finRecibosAppService(
        IRepository<Sapienza.Lexus.finRecibos.finRecibos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finRecibos by Id
    /// </summary>
    public virtual async Task<finRecibosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finRecibos.finRecibos, finRecibosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finReciboses
    /// </summary>
    public virtual async Task<PagedResultDto<finRecibosDto>> GetListAsync(finRecibosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finRecibos.finRecibos>, List<finRecibosDto>>(entities);

        return new PagedResultDto<finRecibosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finRecibos
    /// </summary>
    [Authorize(finRecibosPermissions.Create)]
    public virtual async Task<finRecibosDto> CreateAsync(CreateUpdatefinRecibosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinRecibosDto, Sapienza.Lexus.finRecibos.finRecibos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRecibos.finRecibos, finRecibosDto>(entity);
    }

    /// <summary>
    /// Updates an existing finRecibos
    /// </summary>
    [Authorize(finRecibosPermissions.Update)]
    public virtual async Task<finRecibosDto> UpdateAsync(Guid id, CreateUpdatefinRecibosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finRecibos.finRecibos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finRecibos.finRecibos, finRecibosDto>(entity);
    }

    /// <summary>
    /// Deletes a finRecibos
    /// </summary>
    [Authorize(finRecibosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinRecibosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.referente
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.finRecibos.finRecibos> ApplyFilters(IQueryable<Sapienza.Lexus.finRecibos.finRecibos> queryable, finRecibosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.referente.Contains(input.Filter))
            .WhereIf(input.idRecibo != null, x => x.idRecibo == input.idRecibo)
            .WhereIf(input.idLancamento != null, x => x.idLancamento == input.idLancamento)
            .WhereIf(input.numero != null, x => x.numero == input.numero)
            .WhereIf(!input.referente.IsNullOrWhiteSpace(), x => x.referente.Contains(input.referente))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
