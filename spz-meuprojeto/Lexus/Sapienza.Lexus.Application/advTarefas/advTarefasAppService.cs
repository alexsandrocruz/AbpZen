using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advTarefas.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advTarefas;

/// <summary>
/// Application service for advTarefas entity
/// </summary>
[Authorize(advTarefasPermissions.Default)]
public class advTarefasAppService :
    LexusAppService,
    IadvTarefasAppService
{
    private readonly IRepository<Sapienza.Lexus.advTarefas.advTarefas, Guid> _repository;

    public advTarefasAppService(
        IRepository<Sapienza.Lexus.advTarefas.advTarefas, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advTarefas by Id
    /// </summary>
    public virtual async Task<advTarefasDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advTarefas.advTarefas, advTarefasDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advTarefases
    /// </summary>
    public virtual async Task<PagedResultDto<advTarefasDto>> GetListAsync(advTarefasGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advTarefas.advTarefas>, List<advTarefasDto>>(entities);

        return new PagedResultDto<advTarefasDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advTarefas
    /// </summary>
    [Authorize(advTarefasPermissions.Create)]
    public virtual async Task<advTarefasDto> CreateAsync(CreateUpdateadvTarefasDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvTarefasDto, Sapienza.Lexus.advTarefas.advTarefas>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advTarefas.advTarefas, advTarefasDto>(entity);
    }

    /// <summary>
    /// Updates an existing advTarefas
    /// </summary>
    [Authorize(advTarefasPermissions.Update)]
    public virtual async Task<advTarefasDto> UpdateAsync(Guid id, CreateUpdateadvTarefasDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advTarefas.advTarefas), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advTarefas.advTarefas, advTarefasDto>(entity);
    }

    /// <summary>
    /// Deletes a advTarefas
    /// </summary>
    [Authorize(advTarefasPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvTarefasLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.dataCadastro
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advTarefas.advTarefas> ApplyFilters(IQueryable<Sapienza.Lexus.advTarefas.advTarefas> queryable, advTarefasGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.dataCadastro.Contains(input.Filter) || x.dataParaFinalizacao.Contains(input.Filter) || x.descricao.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter) || x.onde.Contains(input.Filter))
            .WhereIf(input.idTarefa != null, x => x.idTarefa == input.idTarefa)
            .WhereIf(input.idTipoTarefa != null, x => x.idTipoTarefa == input.idTipoTarefa)
            .WhereIf(input.idCompromisso != null, x => x.idCompromisso == input.idCompromisso)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(!input.dataCadastro.IsNullOrWhiteSpace(), x => x.dataCadastro.Contains(input.dataCadastro))
            .WhereIf(!input.dataParaFinalizacao.IsNullOrWhiteSpace(), x => x.dataParaFinalizacao.Contains(input.dataParaFinalizacao))
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(input.ResponsibleUserId != null, x => x.ResponsibleUserId == input.ResponsibleUserId)
            .WhereIf(input.idExecutor != null, x => x.idExecutor == input.idExecutor)
            .WhereIf(input.finalizado != null, x => x.finalizado == input.finalizado)
            .WhereIf(input.tsFinalizacao != null, x => x.tsFinalizacao == input.tsFinalizacao)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            .WhereIf(input.agendada != null, x => x.agendada == input.agendada)
            .WhereIf(input.horarioInicial != null, x => x.horarioInicial == input.horarioInicial)
            .WhereIf(input.horarioFinal != null, x => x.horarioFinal == input.horarioFinal)
            .WhereIf(!input.onde.IsNullOrWhiteSpace(), x => x.onde.Contains(input.onde))
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idUsuarioFinalizou != null, x => x.idUsuarioFinalizou == input.idUsuarioFinalizou)
            .WhereIf(input.lembreteQuandoFinalizarPara != null, x => x.lembreteQuandoFinalizarPara == input.lembreteQuandoFinalizarPara)
            .WhereIf(input.tecnica != null, x => x.tecnica == input.tecnica)
            .WhereIf(input.coletivoOriginal != null, x => x.coletivoOriginal == input.coletivoOriginal)
            .WhereIf(input.coletivoIdOriginal != null, x => x.coletivoIdOriginal == input.coletivoIdOriginal)
            .WhereIf(input.coletivoIdCliente != null, x => x.coletivoIdCliente == input.coletivoIdCliente)
            .WhereIf(input.pauta != null, x => x.pauta == input.pauta)
            .WhereIf(input.pautaIdUsuarioResp != null, x => x.pautaIdUsuarioResp == input.pautaIdUsuarioResp)
            .WhereIf(input.pautaRespAceite != null, x => x.pautaRespAceite == input.pautaRespAceite)
            // ========== FK Filters ==========
            ;
    }
}
