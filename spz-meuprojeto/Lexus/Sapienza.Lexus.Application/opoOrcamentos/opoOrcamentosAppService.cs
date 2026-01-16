using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.opoOrcamentos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.opoOrcamentos;

/// <summary>
/// Application service for opoOrcamentos entity
/// </summary>
[Authorize(opoOrcamentosPermissions.Default)]
public class opoOrcamentosAppService :
    LexusAppService,
    IopoOrcamentosAppService
{
    private readonly IRepository<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, Guid> _repository;

    public opoOrcamentosAppService(
        IRepository<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single opoOrcamentos by Id
    /// </summary>
    public virtual async Task<opoOrcamentosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, opoOrcamentosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of opoOrcamentoses
    /// </summary>
    public virtual async Task<PagedResultDto<opoOrcamentosDto>> GetListAsync(opoOrcamentosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.opoOrcamentos.opoOrcamentos>, List<opoOrcamentosDto>>(entities);

        return new PagedResultDto<opoOrcamentosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new opoOrcamentos
    /// </summary>
    [Authorize(opoOrcamentosPermissions.Create)]
    public virtual async Task<opoOrcamentosDto> CreateAsync(CreateUpdateopoOrcamentosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateopoOrcamentosDto, Sapienza.Lexus.opoOrcamentos.opoOrcamentos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, opoOrcamentosDto>(entity);
    }

    /// <summary>
    /// Updates an existing opoOrcamentos
    /// </summary>
    [Authorize(opoOrcamentosPermissions.Update)]
    public virtual async Task<opoOrcamentosDto> UpdateAsync(Guid id, CreateUpdateopoOrcamentosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.opoOrcamentos.opoOrcamentos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoOrcamentos.opoOrcamentos, opoOrcamentosDto>(entity);
    }

    /// <summary>
    /// Deletes a opoOrcamentos
    /// </summary>
    [Authorize(opoOrcamentosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetopoOrcamentosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.opoOrcamentos.opoOrcamentos> ApplyFilters(IQueryable<Sapienza.Lexus.opoOrcamentos.opoOrcamentos> queryable, opoOrcamentosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.dataCriacao.Contains(input.Filter) || x.arquivo.Contains(input.Filter) || x.dataValidade.Contains(input.Filter) || x.informacoes.Contains(input.Filter) || x.dataPrevistaEntrega.Contains(input.Filter) || x.moeda.Contains(input.Filter) || x.imprimeMoedaAdd.Contains(input.Filter))
            .WhereIf(input.idOrcamento != null, x => x.idOrcamento == input.idOrcamento)
            .WhereIf(input.idOportunidade != null, x => x.idOportunidade == input.idOportunidade)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.dataCriacao.IsNullOrWhiteSpace(), x => x.dataCriacao.Contains(input.dataCriacao))
            .WhereIf(input.valor != null, x => x.valor == input.valor)
            .WhereIf(!input.arquivo.IsNullOrWhiteSpace(), x => x.arquivo.Contains(input.arquivo))
            .WhereIf(input.aceito != null, x => x.aceito == input.aceito)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.dataValidade.IsNullOrWhiteSpace(), x => x.dataValidade.Contains(input.dataValidade))
            .WhereIf(input.valorMensal != null, x => x.valorMensal == input.valorMensal)
            .WhereIf(input.comArquivo != null, x => x.comArquivo == input.comArquivo)
            .WhereIf(input.comProduto != null, x => x.comProduto == input.comProduto)
            .WhereIf(input.comProdutoTerceiro != null, x => x.comProdutoTerceiro == input.comProdutoTerceiro)
            .WhereIf(input.comServico != null, x => x.comServico == input.comServico)
            .WhereIf(input.valorDesconto != null, x => x.valorDesconto == input.valorDesconto)
            .WhereIf(input.valorAcrescimo != null, x => x.valorAcrescimo == input.valorAcrescimo)
            .WhereIf(input.valorFrete != null, x => x.valorFrete == input.valorFrete)
            .WhereIf(!input.informacoes.IsNullOrWhiteSpace(), x => x.informacoes.Contains(input.informacoes))
            .WhereIf(input.descontoPercentual != null, x => x.descontoPercentual == input.descontoPercentual)
            .WhereIf(input.valorItens != null, x => x.valorItens == input.valorItens)
            .WhereIf(input.idCondicaoPagamento != null, x => x.idCondicaoPagamento == input.idCondicaoPagamento)
            .WhereIf(!input.dataPrevistaEntrega.IsNullOrWhiteSpace(), x => x.dataPrevistaEntrega.Contains(input.dataPrevistaEntrega))
            .WhereIf(!input.moeda.IsNullOrWhiteSpace(), x => x.moeda.Contains(input.moeda))
            .WhereIf(input.valorConversao != null, x => x.valorConversao == input.valorConversao)
            .WhereIf(!input.imprimeMoedaAdd.IsNullOrWhiteSpace(), x => x.imprimeMoedaAdd.Contains(input.imprimeMoedaAdd))
            .WhereIf(input.valorDescontoMensal != null, x => x.valorDescontoMensal == input.valorDescontoMensal)
            .WhereIf(input.valorAcrescimoMensal != null, x => x.valorAcrescimoMensal == input.valorAcrescimoMensal)
            .WhereIf(input.valorFreteMensal != null, x => x.valorFreteMensal == input.valorFreteMensal)
            .WhereIf(input.descontoPercentualMensal != null, x => x.descontoPercentualMensal == input.descontoPercentualMensal)
            .WhereIf(input.valorItensMensal != null, x => x.valorItensMensal == input.valorItensMensal)
            // ========== FK Filters ==========
            ;
    }
}
