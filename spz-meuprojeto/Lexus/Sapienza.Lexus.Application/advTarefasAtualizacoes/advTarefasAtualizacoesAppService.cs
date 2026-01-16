using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advTarefasAtualizacoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advTarefasAtualizacoes;

/// <summary>
/// Application service for advTarefasAtualizacoes entity
/// </summary>
[Authorize(advTarefasAtualizacoesPermissions.Default)]
public class advTarefasAtualizacoesAppService :
    LexusAppService,
    IadvTarefasAtualizacoesAppService
{
    private readonly IRepository<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, Guid> _repository;

    public advTarefasAtualizacoesAppService(
        IRepository<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advTarefasAtualizacoes by Id
    /// </summary>
    public virtual async Task<advTarefasAtualizacoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, advTarefasAtualizacoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advTarefasAtualizacoeses
    /// </summary>
    public virtual async Task<PagedResultDto<advTarefasAtualizacoesDto>> GetListAsync(advTarefasAtualizacoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes>, List<advTarefasAtualizacoesDto>>(entities);

        return new PagedResultDto<advTarefasAtualizacoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advTarefasAtualizacoes
    /// </summary>
    [Authorize(advTarefasAtualizacoesPermissions.Create)]
    public virtual async Task<advTarefasAtualizacoesDto> CreateAsync(CreateUpdateadvTarefasAtualizacoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvTarefasAtualizacoesDto, Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, advTarefasAtualizacoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing advTarefasAtualizacoes
    /// </summary>
    [Authorize(advTarefasAtualizacoesPermissions.Update)]
    public virtual async Task<advTarefasAtualizacoesDto> UpdateAsync(Guid id, CreateUpdateadvTarefasAtualizacoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes, advTarefasAtualizacoesDto>(entity);
    }

    /// <summary>
    /// Deletes a advTarefasAtualizacoes
    /// </summary>
    [Authorize(advTarefasAtualizacoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvTarefasAtualizacoesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes> ApplyFilters(IQueryable<Sapienza.Lexus.advTarefasAtualizacoes.advTarefasAtualizacoes> queryable, advTarefasAtualizacoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.campo.Contains(input.Filter) || x.dadoAnterior.Contains(input.Filter))
            .WhereIf(input.idAtualizacaoTarefa != null, x => x.idAtualizacaoTarefa == input.idAtualizacaoTarefa)
            .WhereIf(input.idTarefa != null, x => x.idTarefa == input.idTarefa)
            .WhereIf(input.idCompromisso != null, x => x.idCompromisso == input.idCompromisso)
            .WhereIf(!input.campo.IsNullOrWhiteSpace(), x => x.campo.Contains(input.campo))
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.dadoAnterior.IsNullOrWhiteSpace(), x => x.dadoAnterior.Contains(input.dadoAnterior))
            .WhereIf(input.IdentityUserId != null, x => x.IdentityUserId == input.IdentityUserId)
            // ========== FK Filters ==========
            ;
    }
}
