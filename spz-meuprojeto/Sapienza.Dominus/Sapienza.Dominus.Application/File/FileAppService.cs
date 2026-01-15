using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.File.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.File;

/// <summary>
/// Application service for File entity
/// </summary>
[Authorize(FilePermissions.Default)]
public class FileAppService :
    DominusAppService,
    IFileAppService
{
    private readonly IRepository<Dominus.File.File, Guid> _repository;

    public FileAppService(
        IRepository<Dominus.File.File, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single File by Id
    /// </summary>
    public virtual async Task<FileDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.File.File, FileDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Files
    /// </summary>
    public virtual async Task<PagedResultDto<FileDto>> GetListAsync(FileGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.File.File>, List<FileDto>>(entities);

        return new PagedResultDto<FileDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new File
    /// </summary>
    [Authorize(FilePermissions.Create)]
    public virtual async Task<FileDto> CreateAsync(CreateUpdateFileDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateFileDto, Dominus.File.File>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.File.File, FileDto>(entity);
    }

    /// <summary>
    /// Updates an existing File
    /// </summary>
    [Authorize(FilePermissions.Update)]
    public virtual async Task<FileDto> UpdateAsync(Guid id, CreateUpdateFileDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.File.File), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.File.File, FileDto>(entity);
    }

    /// <summary>
    /// Deletes a File
    /// </summary>
    [Authorize(FilePermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetFileLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.FileName
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.File.File> ApplyFilters(IQueryable<Dominus.File.File> queryable, FileGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
