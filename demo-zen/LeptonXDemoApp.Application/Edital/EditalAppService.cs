using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeptonXDemoApp.Permissions;
using LeptonXDemoApp.Edital.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace LeptonXDemoApp.Edital;

/// <summary>
/// Application service for Edital entity
/// </summary>
[Authorize(LeptonXDemoAppPermissions.Edital.Default)]
public class EditalAppService :
    LeptonXDemoAppAppService,
    IEditalAppService
{
    private readonly IRepository<LeptonXDemoApp.Edital.Edital, Guid> _repository;

    public EditalAppService(
        IRepository<LeptonXDemoApp.Edital.Edital, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Edital by Id
    /// </summary>
    public virtual async Task<EditalDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<LeptonXDemoApp.Edital.Edital, EditalDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Editais
    /// </summary>
    public virtual async Task<PagedResultDto<EditalDto>> GetListAsync(EditalGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<LeptonXDemoApp.Edital.Edital>, List<EditalDto>>(entities);

        return new PagedResultDto<EditalDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Edital
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Edital.Create)]
    public virtual async Task<EditalDto> CreateAsync(CreateUpdateEditalDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateEditalDto, LeptonXDemoApp.Edital.Edital>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Edital.Edital, EditalDto>(entity);
    }

    /// <summary>
    /// Updates an existing Edital
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Edital.Update)]
    public virtual async Task<EditalDto> UpdateAsync(Guid id, CreateUpdateEditalDto input)
    {
        // Fetch with details for Master-Detail update
        var query = await _repository.WithDetailsAsync();
        var entity = await AsyncExecuter.FirstOrDefaultAsync(query, x => x.Id == id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(LeptonXDemoApp.Edital.Edital), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<LeptonXDemoApp.Edital.Edital, EditalDto>(entity);
    }

    /// <summary>
    /// Deletes a Edital
    /// </summary>
    [Authorize(LeptonXDemoAppPermissions.Edital.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetEditalLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Objeto
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<LeptonXDemoApp.Edital.Edital> ApplyFilters(IQueryable<LeptonXDemoApp.Edital.Edital> queryable, EditalGetListInput input)
    {
        return queryable
            .WhereIf(!input.Objeto.IsNullOrWhiteSpace(), x => x.Objeto.Contains(input.Objeto))
            .WhereIf(input.Data != null, x => x.Data == input.Data)
            .WhereIf(input.Valor != null, x => x.Valor == input.Valor)
            // ========== FK Filters ==========
            ;
    }
}
