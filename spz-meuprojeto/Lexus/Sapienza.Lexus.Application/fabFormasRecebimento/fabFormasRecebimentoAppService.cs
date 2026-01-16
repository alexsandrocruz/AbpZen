using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fabFormasRecebimento.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fabFormasRecebimento;

/// <summary>
/// Application service for fabFormasRecebimento entity
/// </summary>
[Authorize(fabFormasRecebimentoPermissions.Default)]
public class fabFormasRecebimentoAppService :
    LexusAppService,
    IfabFormasRecebimentoAppService
{
    private readonly IRepository<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, Guid> _repository;

    public fabFormasRecebimentoAppService(
        IRepository<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fabFormasRecebimento by Id
    /// </summary>
    public virtual async Task<fabFormasRecebimentoDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, fabFormasRecebimentoDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fabFormasRecebimentos
    /// </summary>
    public virtual async Task<PagedResultDto<fabFormasRecebimentoDto>> GetListAsync(fabFormasRecebimentoGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento>, List<fabFormasRecebimentoDto>>(entities);

        return new PagedResultDto<fabFormasRecebimentoDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fabFormasRecebimento
    /// </summary>
    [Authorize(fabFormasRecebimentoPermissions.Create)]
    public virtual async Task<fabFormasRecebimentoDto> CreateAsync(CreateUpdatefabFormasRecebimentoDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefabFormasRecebimentoDto, Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, fabFormasRecebimentoDto>(entity);
    }

    /// <summary>
    /// Updates an existing fabFormasRecebimento
    /// </summary>
    [Authorize(fabFormasRecebimentoPermissions.Update)]
    public virtual async Task<fabFormasRecebimentoDto> UpdateAsync(Guid id, CreateUpdatefabFormasRecebimentoDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento, fabFormasRecebimentoDto>(entity);
    }

    /// <summary>
    /// Deletes a fabFormasRecebimento
    /// </summary>
    [Authorize(fabFormasRecebimentoPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfabFormasRecebimentoLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento> ApplyFilters(IQueryable<Sapienza.Lexus.fabFormasRecebimento.fabFormasRecebimento> queryable, fabFormasRecebimentoGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.tipo.Contains(input.Filter) || x.emailPagSeguro.Contains(input.Filter) || x.texto.Contains(input.Filter) || x.descontoTipo.Contains(input.Filter))
            .WhereIf(input.idFormaRecebimento != null, x => x.idFormaRecebimento == input.idFormaRecebimento)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            .WhereIf(input.padrao != null, x => x.padrao == input.padrao)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idCondicaoPagamento != null, x => x.idCondicaoPagamento == input.idCondicaoPagamento)
            .WhereIf(input.online != null, x => x.online == input.online)
            .WhereIf(!input.tipo.IsNullOrWhiteSpace(), x => x.tipo.Contains(input.tipo))
            .WhereIf(!input.emailPagSeguro.IsNullOrWhiteSpace(), x => x.emailPagSeguro.Contains(input.emailPagSeguro))
            .WhereIf(!input.texto.IsNullOrWhiteSpace(), x => x.texto.Contains(input.texto))
            .WhereIf(input.contasReceber != null, x => x.contasReceber == input.contasReceber)
            .WhereIf(input.vendas != null, x => x.vendas == input.vendas)
            .WhereIf(input.diasParaPrevisao != null, x => x.diasParaPrevisao == input.diasParaPrevisao)
            .WhereIf(input.valorDesconto != null, x => x.valorDesconto == input.valorDesconto)
            .WhereIf(!input.descontoTipo.IsNullOrWhiteSpace(), x => x.descontoTipo.Contains(input.descontoTipo))
            .WhereIf(input.recebimentoFuturo != null, x => x.recebimentoFuturo == input.recebimentoFuturo)
            .WhereIf(input.recebimentoFuturoDias != null, x => x.recebimentoFuturoDias == input.recebimentoFuturoDias)
            .WhereIf(input.recebimentoFuturoTaxa != null, x => x.recebimentoFuturoTaxa == input.recebimentoFuturoTaxa)
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(input.idPlanoConta != null, x => x.idPlanoConta == input.idPlanoConta)
            .WhereIf(input.idCentroCusto != null, x => x.idCentroCusto == input.idCentroCusto)
            .WhereIf(input.idContaPagar != null, x => x.idContaPagar == input.idContaPagar)
            .WhereIf(input.idPlanoContaPagar != null, x => x.idPlanoContaPagar == input.idPlanoContaPagar)
            .WhereIf(input.idCentroCustoPagar != null, x => x.idCentroCustoPagar == input.idCentroCustoPagar)
            .WhereIf(input.idFormaPagar != null, x => x.idFormaPagar == input.idFormaPagar)
            // ========== FK Filters ==========
            ;
    }
}
