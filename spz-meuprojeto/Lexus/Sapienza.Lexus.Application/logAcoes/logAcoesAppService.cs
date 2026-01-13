using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.logAcoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.logAcoes;

/// <summary>
/// Application service for logAcoes entity
/// </summary>
[Authorize(logAcoesPermissions.Default)]
public class logAcoesAppService :
    LexusAppService,
    IlogAcoesAppService
{
    private readonly IRepository<Sapienza.Lexus.logAcoes.logAcoes, Guid> _repository;

    public logAcoesAppService(
        IRepository<Sapienza.Lexus.logAcoes.logAcoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single logAcoes by Id
    /// </summary>
    public virtual async Task<logAcoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.logAcoes.logAcoes, logAcoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of logAcoeses
    /// </summary>
    public virtual async Task<PagedResultDto<logAcoesDto>> GetListAsync(logAcoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.logAcoes.logAcoes>, List<logAcoesDto>>(entities);

        return new PagedResultDto<logAcoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new logAcoes
    /// </summary>
    [Authorize(logAcoesPermissions.Create)]
    public virtual async Task<logAcoesDto> CreateAsync(CreateUpdatelogAcoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatelogAcoesDto, Sapienza.Lexus.logAcoes.logAcoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.logAcoes.logAcoes, logAcoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing logAcoes
    /// </summary>
    [Authorize(logAcoesPermissions.Update)]
    public virtual async Task<logAcoesDto> UpdateAsync(Guid id, CreateUpdatelogAcoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.logAcoes.logAcoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.logAcoes.logAcoes, logAcoesDto>(entity);
    }

    /// <summary>
    /// Deletes a logAcoes
    /// </summary>
    [Authorize(logAcoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetlogAcoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.area
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.logAcoes.logAcoes> ApplyFilters(IQueryable<Sapienza.Lexus.logAcoes.logAcoes> queryable, logAcoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.area.Contains(input.Filter) || x.acao.Contains(input.Filter) || x.usuario.Contains(input.Filter) || x.motivo.Contains(input.Filter))
            .WhereIf(input.idLog != null, x => x.idLog == input.idLog)
            .WhereIf(!input.area.IsNullOrWhiteSpace(), x => x.area.Contains(input.area))
            .WhereIf(!input.acao.IsNullOrWhiteSpace(), x => x.acao.Contains(input.acao))
            .WhereIf(!input.usuario.IsNullOrWhiteSpace(), x => x.usuario.Contains(input.usuario))
            .WhereIf(!input.motivo.IsNullOrWhiteSpace(), x => x.motivo.Contains(input.motivo))
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.idCompromisso != null, x => x.idCompromisso == input.idCompromisso)
            .WhereIf(input.idTarefa != null, x => x.idTarefa == input.idTarefa)
            // ========== FK Filters ==========
            ;
    }
}
