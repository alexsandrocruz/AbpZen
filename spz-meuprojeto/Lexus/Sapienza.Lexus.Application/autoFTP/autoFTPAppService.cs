using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.autoFTP.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.autoFTP;

/// <summary>
/// Application service for autoFTP entity
/// </summary>
[Authorize(autoFTPPermissions.Default)]
public class autoFTPAppService :
    LexusAppService,
    IautoFTPAppService
{
    private readonly IRepository<Sapienza.Lexus.autoFTP.autoFTP, Guid> _repository;

    public autoFTPAppService(
        IRepository<Sapienza.Lexus.autoFTP.autoFTP, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single autoFTP by Id
    /// </summary>
    public virtual async Task<autoFTPDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.autoFTP.autoFTP, autoFTPDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of autoFTPs
    /// </summary>
    public virtual async Task<PagedResultDto<autoFTPDto>> GetListAsync(autoFTPGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.autoFTP.autoFTP>, List<autoFTPDto>>(entities);

        return new PagedResultDto<autoFTPDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new autoFTP
    /// </summary>
    [Authorize(autoFTPPermissions.Create)]
    public virtual async Task<autoFTPDto> CreateAsync(CreateUpdateautoFTPDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateautoFTPDto, Sapienza.Lexus.autoFTP.autoFTP>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.autoFTP.autoFTP, autoFTPDto>(entity);
    }

    /// <summary>
    /// Updates an existing autoFTP
    /// </summary>
    [Authorize(autoFTPPermissions.Update)]
    public virtual async Task<autoFTPDto> UpdateAsync(Guid id, CreateUpdateautoFTPDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.autoFTP.autoFTP), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.autoFTP.autoFTP, autoFTPDto>(entity);
    }

    /// <summary>
    /// Deletes a autoFTP
    /// </summary>
    [Authorize(autoFTPPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetautoFTPLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.arquivo
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.autoFTP.autoFTP> ApplyFilters(IQueryable<Sapienza.Lexus.autoFTP.autoFTP> queryable, autoFTPGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.arquivo.Contains(input.Filter))
            .WhereIf(input.id != null, x => x.id == input.id)
            .WhereIf(!input.arquivo.IsNullOrWhiteSpace(), x => x.arquivo.Contains(input.arquivo))
            .WhereIf(input.processado != null, x => x.processado == input.processado)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            // ========== FK Filters ==========
            ;
    }
}
