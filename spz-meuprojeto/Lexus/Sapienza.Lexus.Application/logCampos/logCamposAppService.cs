using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.logCampos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.logCampos;

/// <summary>
/// Application service for logCampos entity
/// </summary>
[Authorize(logCamposPermissions.Default)]
public class logCamposAppService :
    LexusAppService,
    IlogCamposAppService
{
    private readonly IRepository<Sapienza.Lexus.logCampos.logCampos, Guid> _repository;

    public logCamposAppService(
        IRepository<Sapienza.Lexus.logCampos.logCampos, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single logCampos by Id
    /// </summary>
    public virtual async Task<logCamposDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.logCampos.logCampos, logCamposDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of logCamposes
    /// </summary>
    public virtual async Task<PagedResultDto<logCamposDto>> GetListAsync(logCamposGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.logCampos.logCampos>, List<logCamposDto>>(entities);

        return new PagedResultDto<logCamposDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new logCampos
    /// </summary>
    [Authorize(logCamposPermissions.Create)]
    public virtual async Task<logCamposDto> CreateAsync(CreateUpdatelogCamposDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdatelogCamposDto, Sapienza.Lexus.logCampos.logCampos>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.logCampos.logCampos, logCamposDto>(entity);
    }

    /// <summary>
    /// Updates an existing logCampos
    /// </summary>
    [Authorize(logCamposPermissions.Update)]
    public virtual async Task<logCamposDto> UpdateAsync(Guid id, CreateUpdatelogCamposDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.logCampos.logCampos), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.logCampos.logCampos, logCamposDto>(entity);
    }

    /// <summary>
    /// Deletes a logCampos
    /// </summary>
    [Authorize(logCamposPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetlogCamposLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.campo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.logCampos.logCampos> ApplyFilters(IQueryable<Sapienza.Lexus.logCampos.logCampos> queryable, logCamposGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.campo.Contains(input.Filter) || x.dadoAnterior.Contains(input.Filter) || x.dadoNovo.Contains(input.Filter))
            .WhereIf(input.idLogCampo != null, x => x.idLogCampo == input.idLogCampo)
            .WhereIf(input.idLog != null, x => x.idLog == input.idLog)
            .WhereIf(!input.campo.IsNullOrWhiteSpace(), x => x.campo.Contains(input.campo))
            .WhereIf(!input.dadoAnterior.IsNullOrWhiteSpace(), x => x.dadoAnterior.Contains(input.dadoAnterior))
            .WhereIf(!input.dadoNovo.IsNullOrWhiteSpace(), x => x.dadoNovo.Contains(input.dadoNovo))
            // ========== FK Filters ==========
            ;
    }
}
