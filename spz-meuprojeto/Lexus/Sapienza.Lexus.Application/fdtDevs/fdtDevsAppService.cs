using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.fdtDevs.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.fdtDevs;

/// <summary>
/// Application service for fdtDevs entity
/// </summary>
[Authorize(fdtDevsPermissions.Default)]
public class fdtDevsAppService :
    LexusAppService,
    IfdtDevsAppService
{
    private readonly IRepository<Sapienza.Lexus.fdtDevs.fdtDevs, Guid> _repository;

    public fdtDevsAppService(
        IRepository<Sapienza.Lexus.fdtDevs.fdtDevs, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single fdtDevs by Id
    /// </summary>
    public virtual async Task<fdtDevsDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.fdtDevs.fdtDevs, fdtDevsDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of fdtDevses
    /// </summary>
    public virtual async Task<PagedResultDto<fdtDevsDto>> GetListAsync(fdtDevsGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.fdtDevs.fdtDevs>, List<fdtDevsDto>>(entities);

        return new PagedResultDto<fdtDevsDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new fdtDevs
    /// </summary>
    [Authorize(fdtDevsPermissions.Create)]
    public virtual async Task<fdtDevsDto> CreateAsync(CreateUpdatefdtDevsDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefdtDevsDto, Sapienza.Lexus.fdtDevs.fdtDevs>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fdtDevs.fdtDevs, fdtDevsDto>(entity);
    }

    /// <summary>
    /// Updates an existing fdtDevs
    /// </summary>
    [Authorize(fdtDevsPermissions.Update)]
    public virtual async Task<fdtDevsDto> UpdateAsync(Guid id, CreateUpdatefdtDevsDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.fdtDevs.fdtDevs), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.fdtDevs.fdtDevs, fdtDevsDto>(entity);
    }

    /// <summary>
    /// Deletes a fdtDevs
    /// </summary>
    [Authorize(fdtDevsPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfdtDevsLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.pacote
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.fdtDevs.fdtDevs> ApplyFilters(IQueryable<Sapienza.Lexus.fdtDevs.fdtDevs> queryable, fdtDevsGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.pacote.Contains(input.Filter) || x.descricao.Contains(input.Filter) || x.comentariosRevisor.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter))
            .WhereIf(input.idDev != null, x => x.idDev == input.idDev)
            .WhereIf(!input.pacote.IsNullOrWhiteSpace(), x => x.pacote.Contains(input.pacote))
            .WhereIf(!input.descricao.IsNullOrWhiteSpace(), x => x.descricao.Contains(input.descricao))
            .WhereIf(input.pendente != null, x => x.pendente == input.pendente)
            .WhereIf(input.aprovado != null, x => x.aprovado == input.aprovado)
            .WhereIf(input.reprovado != null, x => x.reprovado == input.reprovado)
            .WhereIf(input.finalizado != null, x => x.finalizado == input.finalizado)
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
