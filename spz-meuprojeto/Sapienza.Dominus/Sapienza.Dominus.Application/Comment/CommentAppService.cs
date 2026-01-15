using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.Comment.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.Comment;

/// <summary>
/// Application service for Comment entity
/// </summary>
[Authorize(CommentPermissions.Default)]
public class CommentAppService :
    DominusAppService,
    ICommentAppService
{
    private readonly IRepository<Dominus.Comment.Comment, Guid> _repository;

    public CommentAppService(
        IRepository<Dominus.Comment.Comment, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single Comment by Id
    /// </summary>
    public virtual async Task<CommentDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.Comment.Comment, CommentDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of Comments
    /// </summary>
    public virtual async Task<PagedResultDto<CommentDto>> GetListAsync(CommentGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.Comment.Comment>, List<CommentDto>>(entities);

        return new PagedResultDto<CommentDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new Comment
    /// </summary>
    [Authorize(CommentPermissions.Create)]
    public virtual async Task<CommentDto> CreateAsync(CreateUpdateCommentDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateCommentDto, Dominus.Comment.Comment>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Comment.Comment, CommentDto>(entity);
    }

    /// <summary>
    /// Updates an existing Comment
    /// </summary>
    [Authorize(CommentPermissions.Update)]
    public virtual async Task<CommentDto> UpdateAsync(Guid id, CreateUpdateCommentDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.Comment.Comment), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.Comment.Comment, CommentDto>(entity);
    }

    /// <summary>
    /// Deletes a Comment
    /// </summary>
    [Authorize(CommentPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetCommentLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.EntityType
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.Comment.Comment> ApplyFilters(IQueryable<Dominus.Comment.Comment> queryable, CommentGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            ;
    }
}
