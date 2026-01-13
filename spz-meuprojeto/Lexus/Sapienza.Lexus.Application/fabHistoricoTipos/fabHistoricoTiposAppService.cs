using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabHistoricoTipos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabHistoricoTipos;

/// <summary>
/// Application service for fabHistoricoTipos entity
/// </summary>
[Authorize(fabHistoricoTiposPermissions.Default)]
public class fabHistoricoTiposAppService :
    LexusAppService,
    IfabHistoricoTiposAppService
{
    private readonly IRepository<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, Guid> _repository;

    public fabHistoricoTiposAppService(
        IRepository<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabHistoricoTipos by Id
    /// </summary>
    public virtual async Task<fabHistoricoTiposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabHistoricoTiposes
    /// </summary>
    public virtual async Task<PagedResultDto<fabHistoricoTiposDto>> GetListAsync(fabHistoricoTiposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>, List<fabHistoricoTiposDto>>(entities);

        return new PagedResultDto<fabHistoricoTiposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Create)]
    public virtual async Task<fabHistoricoTiposDto> CreateAsync(CreateUpdatefabHistoricoTiposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabHistoricoTiposDto, Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Update)]
    public virtual async Task<fabHistoricoTiposDto> UpdateAsync(Guid id, CreateUpdatefabHistoricoTiposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos, fabHistoricoTiposDto>(entity);
    }

    /// <summary>
    /// Deletes a fabHistoricoTipos
    /// </summary>
    [Authorize(fabHistoricoTiposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabHistoricoTiposLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> ApplyFilters(IQueryable<Sapienza.Lexus.fabHistoricoTipos.fabHistoricoTipos> queryable, fabHistoricoTiposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.tipoMarcacoes.Contains(input.Filter))
            .WhereIf(input.idHistoricoTipo != null, x => x.idHistoricoTipo == input.idHistoricoTipo)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.tipoMarcacoes.IsNullOrWhiteSpace(), x => x.tipoMarcacoes.Contains(input.tipoMarcacoes))
            // ========== FK Filters ==========
            ;
    }
}
