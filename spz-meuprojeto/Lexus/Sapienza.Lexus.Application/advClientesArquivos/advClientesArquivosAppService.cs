using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesArquivos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesArquivos;

/// <summary>
/// Application service for advClientesArquivos entity
/// </summary>
[Authorize(advClientesArquivosPermissions.Default)]
public class advClientesArquivosAppService :
    LexusAppService,
    IadvClientesArquivosAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, Guid> _repository;

    public advClientesArquivosAppService(
        IRepository<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesArquivos by Id
    /// </summary>
    public virtual async Task<advClientesArquivosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, advClientesArquivosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesArquivoses
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesArquivosDto>> GetListAsync(advClientesArquivosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesArquivos.advClientesArquivos>, List<advClientesArquivosDto>>(entities);

        return new PagedResultDto<advClientesArquivosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesArquivos
    /// </summary>
    [Authorize(advClientesArquivosPermissions.Create)]
    public virtual async Task<advClientesArquivosDto> CreateAsync(CreateUpdateadvClientesArquivosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesArquivosDto, Sapienza.Lexus.advClientesArquivos.advClientesArquivos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, advClientesArquivosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesArquivos
    /// </summary>
    [Authorize(advClientesArquivosPermissions.Update)]
    public virtual async Task<advClientesArquivosDto> UpdateAsync(Guid id, CreateUpdateadvClientesArquivosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesArquivos.advClientesArquivos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesArquivos.advClientesArquivos, advClientesArquivosDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesArquivos
    /// </summary>
    [Authorize(advClientesArquivosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesArquivosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.descricao
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesArquivos.advClientesArquivos> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesArquivos.advClientesArquivos> queryable, advClientesArquivosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.descricao.Contains(input.Filter) || x.arquivo.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter) || x.solicitanteComentario.Contains(input.Filter) || x.revisorComentario.Contains(input.Filter) || x.status.Contains(input.Filter))
            .WhereIf(input.idArquivo != null, x => x.idArquivo == input.idArquivo)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idTipoArquivo != null, x => x.idTipoArquivo == input.idTipoArquivo)
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(!input.arquivo.IsNullOrWhiteSpace(), x => x.arquivo.Contains(input.arquivo))
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(input.precisaRevisao != null, x => x.precisaRevisao == input.precisaRevisao)
            .WhereIf(input.idSolicitante != null, x => x.idSolicitante == input.idSolicitante)
            .WhereIf(!input.solicitanteComentario.IsNullOrWhiteSpace(), x => x.solicitanteComentario.Contains(input.solicitanteComentario))
            .WhereIf(input.idRevisor != null, x => x.idRevisor == input.idRevisor)
            .WhereIf(!input.revisorComentario.IsNullOrWhiteSpace(), x => x.revisorComentario.Contains(input.revisorComentario))
            .WhereIf(input.reprovado != null, x => x.reprovado == input.reprovado)
            .WhereIf(input.pendenteVisualizacaoAprovacao != null, x => x.pendenteVisualizacaoAprovacao == input.pendenteVisualizacaoAprovacao)
            .WhereIf(!input.status.IsNullOrWhiteSpace(), x => x.status.Contains(input.status))
            .WhereIf(input.autoFTP != null, x => x.autoFTP == input.autoFTP)
            // ========== FK Filters ==========
            ;
    }
}
