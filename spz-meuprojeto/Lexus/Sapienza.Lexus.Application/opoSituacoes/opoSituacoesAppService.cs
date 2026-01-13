using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.opoSituacoes.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.opoSituacoes;

/// <summary>
/// Application service for opoSituacoes entity
/// </summary>
[Authorize(opoSituacoesPermissions.Default)]
public class opoSituacoesAppService :
    LexusAppService,
    IopoSituacoesAppService
{
    private readonly IRepository<Sapienza.Lexus.opoSituacoes.opoSituacoes, Guid> _repository;

    public opoSituacoesAppService(
        IRepository<Sapienza.Lexus.opoSituacoes.opoSituacoes, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single opoSituacoes by Id
    /// </summary>
    public virtual async Task<opoSituacoesDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.opoSituacoes.opoSituacoes, opoSituacoesDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of opoSituacoeses
    /// </summary>
    public virtual async Task<PagedResultDto<opoSituacoesDto>> GetListAsync(opoSituacoesGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.opoSituacoes.opoSituacoes>, List<opoSituacoesDto>>(entities);

        return new PagedResultDto<opoSituacoesDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new opoSituacoes
    /// </summary>
    [Authorize(opoSituacoesPermissions.Create)]
    public virtual async Task<opoSituacoesDto> CreateAsync(CreateUpdateopoSituacoesDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateopoSituacoesDto, Sapienza.Lexus.opoSituacoes.opoSituacoes>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoSituacoes.opoSituacoes, opoSituacoesDto>(entity);
    }

    /// <summary>
    /// Updates an existing opoSituacoes
    /// </summary>
    [Authorize(opoSituacoesPermissions.Update)]
    public virtual async Task<opoSituacoesDto> UpdateAsync(Guid id, CreateUpdateopoSituacoesDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.opoSituacoes.opoSituacoes), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.opoSituacoes.opoSituacoes, opoSituacoesDto>(entity);
    }

    /// <summary>
    /// Deletes a opoSituacoes
    /// </summary>
    [Authorize(opoSituacoesPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetopoSituacoesLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.opoSituacoes.opoSituacoes> ApplyFilters(IQueryable<Sapienza.Lexus.opoSituacoes.opoSituacoes> queryable, opoSituacoesGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idSituacao != null, x => x.idSituacao == input.idSituacao)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.ordem != null, x => x.ordem == input.ordem)
            .WhereIf(input.considerarIndicador != null, x => x.considerarIndicador == input.considerarIndicador)
            // ========== FK Filters ==========
            ;
    }
}
