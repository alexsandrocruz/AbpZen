using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advPreArquivosStatus.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advPreArquivosStatus;

/// <summary>
/// Application service for advPreArquivosStatus entity
/// </summary>
[Authorize(advPreArquivosStatusPermissions.Default)]
public class advPreArquivosStatusAppService :
    LexusAppService,
    IadvPreArquivosStatusAppService
{
    private readonly IRepository<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, Guid> _repository;

    public advPreArquivosStatusAppService(
        IRepository<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advPreArquivosStatus by Id
    /// </summary>
    public virtual async Task<advPreArquivosStatusDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, advPreArquivosStatusDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advPreArquivosStatuses
    /// </summary>
    public virtual async Task<PagedResultDto<advPreArquivosStatusDto>> GetListAsync(advPreArquivosStatusGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus>, List<advPreArquivosStatusDto>>(entities);

        return new PagedResultDto<advPreArquivosStatusDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advPreArquivosStatus
    /// </summary>
    [Authorize(advPreArquivosStatusPermissions.Create)]
    public virtual async Task<advPreArquivosStatusDto> CreateAsync(CreateUpdateadvPreArquivosStatusDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvPreArquivosStatusDto, Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, advPreArquivosStatusDto>(entity);
    }

    /// <summary>
    /// Updates an existing advPreArquivosStatus
    /// </summary>
    [Authorize(advPreArquivosStatusPermissions.Update)]
    public virtual async Task<advPreArquivosStatusDto> UpdateAsync(Guid id, CreateUpdateadvPreArquivosStatusDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus, advPreArquivosStatusDto>(entity);
    }

    /// <summary>
    /// Deletes a advPreArquivosStatus
    /// </summary>
    [Authorize(advPreArquivosStatusPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvPreArquivosStatusLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus> ApplyFilters(IQueryable<Sapienza.Lexus.advPreArquivosStatus.advPreArquivosStatus> queryable, advPreArquivosStatusGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.titulo.Contains(input.Filter))
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(!input.titulo.IsNullOrWhiteSpace(), x => x.titulo.Contains(input.titulo))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            // ========== FK Filters ==========
            ;
    }
}
