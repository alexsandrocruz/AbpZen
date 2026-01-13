using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.opoOportunidades.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.opoOportunidades;

/// <summary>
/// Application service for opoOportunidades entity
/// </summary>
[Authorize(opoOportunidadesPermissions.Default)]
public class opoOportunidadesAppService :
    LexusAppService,
    IopoOportunidadesAppService
{
    private readonly IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> _repository;

    public opoOportunidadesAppService(
        IRepository<Sapienza.Lexus.opoOportunidades.opoOportunidades, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single opoOportunidades by Id
    /// </summary>
    public virtual async Task<opoOportunidadesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.opoOportunidades.opoOportunidades, opoOportunidadesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of opoOportunidadeses
    /// </summary>
    public virtual async Task<PagedResultDto<opoOportunidadesDto>> GetListAsync(opoOportunidadesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.opoOportunidades.opoOportunidades>, List<opoOportunidadesDto>>(entities);

        return new PagedResultDto<opoOportunidadesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new opoOportunidades
    /// </summary>
    [Authorize(opoOportunidadesPermissions.Create)]
    public virtual async Task<opoOportunidadesDto> CreateAsync(CreateUpdateopoOportunidadesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateopoOportunidadesDto, Sapienza.Lexus.opoOportunidades.opoOportunidades>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoOportunidades.opoOportunidades, opoOportunidadesDto>(entity);
    }

    /// <summary>
    /// Updates an existing opoOportunidades
    /// </summary>
    [Authorize(opoOportunidadesPermissions.Update)]
    public virtual async Task<opoOportunidadesDto> UpdateAsync(Guid id, CreateUpdateopoOportunidadesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.opoOportunidades.opoOportunidades), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoOportunidades.opoOportunidades, opoOportunidadesDto>(entity);
    }

    /// <summary>
    /// Deletes a opoOportunidades
    /// </summary>
    [Authorize(opoOportunidadesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetopoOportunidadesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.opoOportunidades.opoOportunidades> ApplyFilters(IQueryable<Sapienza.Lexus.opoOportunidades.opoOportunidades> queryable, opoOportunidadesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.dataInicio.Contains(input.Filter) || x.dataEstimada.Contains(input.Filter) || x.comentario.Contains(input.Filter) || x.aproveitadaData.Contains(input.Filter) || x.canceladaMotivo.Contains(input.Filter) || x.canceladaData.Contains(input.Filter) || x.aproveitadaMotivo.Contains(input.Filter))
            .WhereIf(input.idOportunidade != null, x => x.idOportunidade == input.idOportunidade)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(input.idSituacao != null, x => x.idSituacao == input.idSituacao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.dataInicio.IsNullOrWhiteSpace(), x => x.dataInicio.Contains(input.dataInicio))
            .WhereIf(!input.dataEstimada.IsNullOrWhiteSpace(), x => x.dataEstimada.Contains(input.dataEstimada))
            .WhereIf(input.valorEstimado != null, x => x.valorEstimado == input.valorEstimado)
            .WhereIf(!input.comentario.IsNullOrWhiteSpace(), x => x.comentario.Contains(input.comentario))
            .WhereIf(input.aproveitada != null, x => x.aproveitada == input.aproveitada)
            .WhereIf(!input.aproveitadaData.IsNullOrWhiteSpace(), x => x.aproveitadaData.Contains(input.aproveitadaData))
            .WhereIf(input.cancelada != null, x => x.cancelada == input.cancelada)
            .WhereIf(!input.canceladaMotivo.IsNullOrWhiteSpace(), x => x.canceladaMotivo.Contains(input.canceladaMotivo))
            .WhereIf(!input.canceladaData.IsNullOrWhiteSpace(), x => x.canceladaData.Contains(input.canceladaData))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.indicadorCanceladoVisto != null, x => x.indicadorCanceladoVisto == input.indicadorCanceladoVisto)
            .WhereIf(input.valorEstimadoMensal != null, x => x.valorEstimadoMensal == input.valorEstimadoMensal)
            .WhereIf(input.deProcesso != null, x => x.deProcesso == input.deProcesso)
            .WhereIf(!input.aproveitadaMotivo.IsNullOrWhiteSpace(), x => x.aproveitadaMotivo.Contains(input.aproveitadaMotivo))
            .WhereIf(input.numeroProcesso != null, x => x.numeroProcesso == input.numeroProcesso)
            // ========== FK Filters ==========
            ;
    }
}
