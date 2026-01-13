using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.finGruposDRE.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.finGruposDRE;

/// <summary>
/// Application service for finGruposDRE entity
/// </summary>
[Authorize(finGruposDREPermissions.Default)]
public class finGruposDREAppService :
    LexusAppService,
    IfinGruposDREAppService
{
    private readonly IRepository<Sapienza.Lexus.finGruposDRE.finGruposDRE, Guid> _repository;

    public finGruposDREAppService(
        IRepository<Sapienza.Lexus.finGruposDRE.finGruposDRE, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single finGruposDRE by Id
    /// </summary>
    public virtual async Task<finGruposDREDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.finGruposDRE.finGruposDRE, finGruposDREDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of finGruposDREs
    /// </summary>
    public virtual async Task<PagedResultDto<finGruposDREDto>> GetListAsync(finGruposDREGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.finGruposDRE.finGruposDRE>, List<finGruposDREDto>>(entities);

        return new PagedResultDto<finGruposDREDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new finGruposDRE
    /// </summary>
    [Authorize(finGruposDREPermissions.Create)]
    public virtual async Task<finGruposDREDto> CreateAsync(CreateUpdatefinGruposDREDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatefinGruposDREDto, Sapienza.Lexus.finGruposDRE.finGruposDRE>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finGruposDRE.finGruposDRE, finGruposDREDto>(entity);
    }

    /// <summary>
    /// Updates an existing finGruposDRE
    /// </summary>
    [Authorize(finGruposDREPermissions.Update)]
    public virtual async Task<finGruposDREDto> UpdateAsync(Guid id, CreateUpdatefinGruposDREDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.finGruposDRE.finGruposDRE), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.finGruposDRE.finGruposDRE, finGruposDREDto>(entity);
    }

    /// <summary>
    /// Deletes a finGruposDRE
    /// </summary>
    [Authorize(finGruposDREPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetfinGruposDRELookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.finGruposDRE.finGruposDRE> ApplyFilters(IQueryable<Sapienza.Lexus.finGruposDRE.finGruposDRE> queryable, finGruposDREGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idGrupoDRE != null, x => x.idGrupoDRE == input.idGrupoDRE)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
