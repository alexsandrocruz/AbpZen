using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.flwFollows.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwFollows;

/// <summary>
/// Application service for flwFollows entity
/// </summary>
[Authorize(flwFollowsPermissions.Default)]
public class flwFollowsAppService :
    LexusAppService,
    IflwFollowsAppService
{
    private readonly IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> _repository;

    public flwFollowsAppService(
        IRepository<Sapienza.Lexus.flwFollows.flwFollows, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single flwFollows by Id
    /// </summary>
    public virtual async Task<flwFollowsDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.flwFollows.flwFollows, flwFollowsDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of flwFollowses
    /// </summary>
    public virtual async Task<PagedResultDto<flwFollowsDto>> GetListAsync(flwFollowsGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.flwFollows.flwFollows>, List<flwFollowsDto>>(entities);

        return new PagedResultDto<flwFollowsDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new flwFollows
    /// </summary>
    [Authorize(flwFollowsPermissions.Create)]
    public virtual async Task<flwFollowsDto> CreateAsync(CreateUpdateflwFollowsDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateflwFollowsDto, Sapienza.Lexus.flwFollows.flwFollows>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwFollows.flwFollows, flwFollowsDto>(entity);
    }

    /// <summary>
    /// Updates an existing flwFollows
    /// </summary>
    [Authorize(flwFollowsPermissions.Update)]
    public virtual async Task<flwFollowsDto> UpdateAsync(Guid id, CreateUpdateflwFollowsDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.flwFollows.flwFollows), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwFollows.flwFollows, flwFollowsDto>(entity);
    }

    /// <summary>
    /// Deletes a flwFollows
    /// </summary>
    [Authorize(flwFollowsPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetflwFollowsLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.data
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.flwFollows.flwFollows> ApplyFilters(IQueryable<Sapienza.Lexus.flwFollows.flwFollows> queryable, flwFollowsGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.data.Contains(input.Filter) || x.comentario.Contains(input.Filter) || x.dataFinalizacao.Contains(input.Filter))
            .WhereIf(input.idFollow != null, x => x.idFollow == input.idFollow)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idAcao != null, x => x.idAcao == input.idAcao)
            .WhereIf(input.idTipo != null, x => x.idTipo == input.idTipo)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(!input.data.IsNullOrWhiteSpace(), x => x.data.Contains(input.data))
            .WhereIf(input.horario != null, x => x.horario == input.horario)
            .WhereIf(!input.comentario.IsNullOrWhiteSpace(), x => x.comentario.Contains(input.comentario))
            .WhereIf(input.finalizado != null, x => x.finalizado == input.finalizado)
            .WhereIf(!input.dataFinalizacao.IsNullOrWhiteSpace(), x => x.dataFinalizacao.Contains(input.dataFinalizacao))
            .WhereIf(input.horarioFinalizacao != null, x => x.horarioFinalizacao == input.horarioFinalizacao)
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idOportunidade != null, x => x.idOportunidade == input.idOportunidade)
            .WhereIf(input.chegou != null, x => x.chegou == input.chegou)
            .WhereIf(input.tsChegou != null, x => x.tsChegou == input.tsChegou)
            .WhereIf(input.naoComparecimento != null, x => x.naoComparecimento == input.naoComparecimento)
            .WhereIf(input.prioridade != null, x => x.prioridade == input.prioridade)
            // ========== FK Filters ==========
            ;
    }
}
