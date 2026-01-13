using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesAtualizacoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesAtualizacoes;

/// <summary>
/// Application service for advClientesAtualizacoes entity
/// </summary>
[Authorize(advClientesAtualizacoesPermissions.Default)]
public class advClientesAtualizacoesAppService :
    LexusAppService,
    IadvClientesAtualizacoesAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, Guid> _repository;

    public advClientesAtualizacoesAppService(
        IRepository<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesAtualizacoes by Id
    /// </summary>
    public virtual async Task<advClientesAtualizacoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, advClientesAtualizacoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesAtualizacoeses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesAtualizacoesDto>> GetListAsync(advClientesAtualizacoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes>, List<advClientesAtualizacoesDto>>(entities);

        return new PagedResultDto<advClientesAtualizacoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesAtualizacoes
    /// </summary>
    [Authorize(advClientesAtualizacoesPermissions.Create)]
    public virtual async Task<advClientesAtualizacoesDto> CreateAsync(CreateUpdateadvClientesAtualizacoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesAtualizacoesDto, Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, advClientesAtualizacoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesAtualizacoes
    /// </summary>
    [Authorize(advClientesAtualizacoesPermissions.Update)]
    public virtual async Task<advClientesAtualizacoesDto> UpdateAsync(Guid id, CreateUpdateadvClientesAtualizacoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes, advClientesAtualizacoesDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesAtualizacoes
    /// </summary>
    [Authorize(advClientesAtualizacoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesAtualizacoesLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.campo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesAtualizacoes.advClientesAtualizacoes> queryable, advClientesAtualizacoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.campo.Contains(input.Filter) || x.dadoAnterior.Contains(input.Filter))
            .WhereIf(input.idAtualizacao != null, x => x.idAtualizacao == input.idAtualizacao)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(!input.campo.IsNullOrWhiteSpace(), x => x.campo.Contains(input.campo))
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.dadoAnterior.IsNullOrWhiteSpace(), x => x.dadoAnterior.Contains(input.dadoAnterior))
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            // ========== FK Filters ==========
            ;
    }
}
