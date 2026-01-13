using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advRevisaoDocumentos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advRevisaoDocumentos;

/// <summary>
/// Application service for advRevisaoDocumentos entity
/// </summary>
[Authorize(advRevisaoDocumentosPermissions.Default)]
public class advRevisaoDocumentosAppService :
    LexusAppService,
    IadvRevisaoDocumentosAppService
{
    private readonly IRepository<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, Guid> _repository;

    public advRevisaoDocumentosAppService(
        IRepository<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advRevisaoDocumentos by Id
    /// </summary>
    public virtual async Task<advRevisaoDocumentosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, advRevisaoDocumentosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advRevisaoDocumentoses
    /// </summary>
    public virtual async Task<PagedResultDto<advRevisaoDocumentosDto>> GetListAsync(advRevisaoDocumentosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos>, List<advRevisaoDocumentosDto>>(entities);

        return new PagedResultDto<advRevisaoDocumentosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advRevisaoDocumentos
    /// </summary>
    [Authorize(advRevisaoDocumentosPermissions.Create)]
    public virtual async Task<advRevisaoDocumentosDto> CreateAsync(CreateUpdateadvRevisaoDocumentosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvRevisaoDocumentosDto, Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, advRevisaoDocumentosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advRevisaoDocumentos
    /// </summary>
    [Authorize(advRevisaoDocumentosPermissions.Update)]
    public virtual async Task<advRevisaoDocumentosDto> UpdateAsync(Guid id, CreateUpdateadvRevisaoDocumentosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos, advRevisaoDocumentosDto>(entity);
    }

    /// <summary>
    /// Deletes a advRevisaoDocumentos
    /// </summary>
    [Authorize(advRevisaoDocumentosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvRevisaoDocumentosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.localRede
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos> ApplyFilters(IQueryable<Sapienza.Lexus.advRevisaoDocumentos.advRevisaoDocumentos> queryable, advRevisaoDocumentosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.localRede.Contains(input.Filter) || x.comentariosSolicitante.Contains(input.Filter) || x.comentariosRevisor.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter))
            .WhereIf(input.idRevisao != null, x => x.idRevisao == input.idRevisao)
            .WhereIf(input.idUsuarioSolicitante != null, x => x.idUsuarioSolicitante == input.idUsuarioSolicitante)
            .WhereIf(input.idUsuarioRevisor != null, x => x.idUsuarioRevisor == input.idUsuarioRevisor)
            .WhereIf(!input.localRede.IsNullOrWhiteSpace(), x => x.localRede.Contains(input.localRede))
            .WhereIf(input.pendente != null, x => x.pendente == input.pendente)
            .WhereIf(input.aprovado != null, x => x.aprovado == input.aprovado)
            .WhereIf(input.reprovado != null, x => x.reprovado == input.reprovado)
            .WhereIf(input.finalizado != null, x => x.finalizado == input.finalizado)
            .WhereIf(!input.comentariosSolicitante.IsNullOrWhiteSpace(), x => x.comentariosSolicitante.Contains(input.comentariosSolicitante))
            .WhereIf(!input.comentariosRevisor.IsNullOrWhiteSpace(), x => x.comentariosRevisor.Contains(input.comentariosRevisor))
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            // ========== FK Filters ==========
            ;
    }
}
