using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCompromissos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCompromissos;

/// <summary>
/// Application service for advCompromissos entity
/// </summary>
[Authorize(advCompromissosPermissions.Default)]
public class advCompromissosAppService :
    LexusAppService,
    IadvCompromissosAppService
{
    private readonly IRepository<Sapienza.Lexus.advCompromissos.advCompromissos, Guid> _repository;

    public advCompromissosAppService(
        IRepository<Sapienza.Lexus.advCompromissos.advCompromissos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCompromissos by Id
    /// </summary>
    public virtual async Task<advCompromissosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCompromissos.advCompromissos, advCompromissosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCompromissoses
    /// </summary>
    public virtual async Task<PagedResultDto<advCompromissosDto>> GetListAsync(advCompromissosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCompromissos.advCompromissos>, List<advCompromissosDto>>(entities);

        return new PagedResultDto<advCompromissosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCompromissos
    /// </summary>
    [Authorize(advCompromissosPermissions.Create)]
    public virtual async Task<advCompromissosDto> CreateAsync(CreateUpdateadvCompromissosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCompromissosDto, Sapienza.Lexus.advCompromissos.advCompromissos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCompromissos.advCompromissos, advCompromissosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCompromissos
    /// </summary>
    [Authorize(advCompromissosPermissions.Update)]
    public virtual async Task<advCompromissosDto> UpdateAsync(Guid id, CreateUpdateadvCompromissosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCompromissos.advCompromissos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCompromissos.advCompromissos, advCompromissosDto>(entity);
    }

    /// <summary>
    /// Deletes a advCompromissos
    /// </summary>
    [Authorize(advCompromissosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCompromissosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.dataPublicacao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advCompromissos.advCompromissos> ApplyFilters(IQueryable<Sapienza.Lexus.advCompromissos.advCompromissos> queryable, advCompromissosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.dataPublicacao.Contains(input.Filter) || x.dataPrazoInterno.Contains(input.Filter) || x.dataPrazoFatal.Contains(input.Filter) || x.descricao.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter))
            .WhereIf(input.idCompromisso != null, x => x.idCompromisso == input.idCompromisso)
            .WhereIf(input.idTipoCompromisso != null, x => x.idTipoCompromisso == input.idTipoCompromisso)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(!input.dataPublicacao.IsNullOrWhiteSpace(), x => x.dataPublicacao.Contains(input.dataPublicacao))
            .WhereIf(!input.dataPrazoInterno.IsNullOrWhiteSpace(), x => x.dataPrazoInterno.Contains(input.dataPrazoInterno))
            .WhereIf(!input.dataPrazoFatal.IsNullOrWhiteSpace(), x => x.dataPrazoFatal.Contains(input.dataPrazoFatal))
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            .WhereIf(input.idAgendamentoINSS != null, x => x.idAgendamentoINSS == input.idAgendamentoINSS)
            .WhereIf(input.pauta != null, x => x.pauta == input.pauta)
            .WhereIf(input.pautaIdUsuarioResp != null, x => x.pautaIdUsuarioResp == input.pautaIdUsuarioResp)
            .WhereIf(input.pautaRespAceite != null, x => x.pautaRespAceite == input.pautaRespAceite)
            .WhereIf(input.horarioInicial != null, x => x.horarioInicial == input.horarioInicial)
            .WhereIf(input.horarioFinal != null, x => x.horarioFinal == input.horarioFinal)
            // ========== FK Filters ==========
            ;
    }
}
