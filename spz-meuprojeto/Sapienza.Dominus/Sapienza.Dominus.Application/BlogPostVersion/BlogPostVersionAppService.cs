using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.BlogPostVersion.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPostVersion;

/// <summary>
/// Application service for BlogPostVersion entity
/// </summary>
[Authorize(BlogPostVersionPermissions.Default)]
public class BlogPostVersionAppService :
    DominusAppService,
    IBlogPostVersionAppService
{
    private readonly IRepository<Dominus.BlogPostVersion.BlogPostVersion, Guid> _repository;
    private readonly IRepository<Dominus.BlogPost.BlogPost, Guid> _blogPostRepository;

    public BlogPostVersionAppService(
        IRepository<Dominus.BlogPostVersion.BlogPostVersion, Guid> repository,
        IRepository<Dominus.BlogPost.BlogPost, Guid> blogPostRepository
    )
    {
        _repository = repository;
        _blogPostRepository = blogPostRepository;
    }

    /// <summary>
    /// Gets a single BlogPostVersion by Id
    /// </summary>
    public virtual async Task<BlogPostVersionDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.BlogPostVersion.BlogPostVersion, BlogPostVersionDto>(entity);
        if (entity.BlogPostId != null)
        {
            var parent = await _blogPostRepository.FindAsync(entity.BlogPostId.Value);
            dto.BlogPostDisplayName = parent?.Title;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of BlogPostVersions
    /// </summary>
    public virtual async Task<PagedResultDto<BlogPostVersionDto>> GetListAsync(BlogPostVersionGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.BlogPostVersion.BlogPostVersion>, List<BlogPostVersionDto>>(entities);
        var blogPostIds = entities
            .Where(x => x.BlogPostId != null)
            .Select(x => x.BlogPostId.Value)
            .Distinct()
            .ToList();

        if (blogPostIds.Any())
        {
            var parents = await _blogPostRepository.GetListAsync(x => blogPostIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Title);

            foreach (var dto in dtoList.Where(x => x.BlogPostId != null))
            {
                if (parentMap.TryGetValue(dto.BlogPostId.Value, out var displayName))
                {
                    dto.BlogPostDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<BlogPostVersionDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new BlogPostVersion
    /// </summary>
    [Authorize(BlogPostVersionPermissions.Create)]
    public virtual async Task<BlogPostVersionDto> CreateAsync(CreateUpdateBlogPostVersionDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateBlogPostVersionDto, Dominus.BlogPostVersion.BlogPostVersion>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogPostVersion.BlogPostVersion, BlogPostVersionDto>(entity);
    }

    /// <summary>
    /// Updates an existing BlogPostVersion
    /// </summary>
    [Authorize(BlogPostVersionPermissions.Update)]
    public virtual async Task<BlogPostVersionDto> UpdateAsync(Guid id, CreateUpdateBlogPostVersionDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.BlogPostVersion.BlogPostVersion), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogPostVersion.BlogPostVersion, BlogPostVersionDto>(entity);
    }

    /// <summary>
    /// Deletes a BlogPostVersion
    /// </summary>
    [Authorize(BlogPostVersionPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBlogPostVersionLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Id.ToString()
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.BlogPostVersion.BlogPostVersion> ApplyFilters(IQueryable<Dominus.BlogPostVersion.BlogPostVersion> queryable, BlogPostVersionGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.BlogPostId != null, x => x.BlogPostId == input.BlogPostId)
            ;
    }
}
