using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliComoChegou.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliComoChegou;

/// <summary>
/// Application service for advCliComoChegou entity
/// </summary>
[Authorize(advCliComoChegouPermissions.Default)]
public class advCliComoChegouAppService :
    LexusAppService,
    IadvCliComoChegouAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliComoChegou.advCliComoChegou, Guid> _repository;

    public advCliComoChegouAppService(
        IRepository<Sapienza.Lexus.advCliComoChegou.advCliComoChegou, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliComoChegou by Id
    /// </summary>
    public virtual async Task<advCliComoChegouDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliComoChegou.advCliComoChegou, advCliComoChegouDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliComoChegous
    /// </summary>
    public virtual async Task<PagedResultDto<advCliComoChegouDto>> GetListAsync(advCliComoChegouGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliComoChegou.advCliComoChegou>, List<advCliComoChegouDto>>(entities);

        return new PagedResultDto<advCliComoChegouDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliComoChegou
    /// </summary>
    [Authorize(advCliComoChegouPermissions.Create)]
    public virtual async Task<advCliComoChegouDto> CreateAsync(CreateUpdateadvCliComoChegouDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliComoChegouDto, Sapienza.Lexus.advCliComoChegou.advCliComoChegou>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliComoChegou.advCliComoChegou, advCliComoChegouDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliComoChegou
    /// </summary>
    [Authorize(advCliComoChegouPermissions.Update)]
    public virtual async Task<advCliComoChegouDto> UpdateAsync(Guid id, CreateUpdateadvCliComoChegouDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliComoChegou.advCliComoChegou), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliComoChegou.advCliComoChegou, advCliComoChegouDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliComoChegou
    /// </summary>
    [Authorize(advCliComoChegouPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliComoChegouLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliComoChegou.advCliComoChegou> ApplyFilters(IQueryable<Sapienza.Lexus.advCliComoChegou.advCliComoChegou> queryable, advCliComoChegouGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idComoChegou != null, x => x.idComoChegou == input.idComoChegou)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
