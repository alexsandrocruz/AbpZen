using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Dominus.Permissions;
using Sapienza.Dominus.BlogPost.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Dominus.BlogPost;

/// <summary>
/// Application service for BlogPost entity
/// </summary>
[Authorize(BlogPostPermissions.Default)]
public class BlogPostAppService :
    DominusAppService,
    IBlogPostAppService
{
    private readonly IRepository<Dominus.BlogPost.BlogPost, Guid> _repository;
    private readonly IRepository<Dominus.Site.Site, Guid> _siteRepository;

    public BlogPostAppService(
        IRepository<Dominus.BlogPost.BlogPost, Guid> repository,
        IRepository<Dominus.Site.Site, Guid> siteRepository
    )
    {
        _repository = repository;
        _siteRepository = siteRepository;
    }

    /// <summary>
    /// Gets a single BlogPost by Id
    /// </summary>
    public virtual async Task<BlogPostDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Dominus.BlogPost.BlogPost, BlogPostDto>(entity);
        if (entity.SiteId != null)
        {
            var parent = await _siteRepository.FindAsync(entity.SiteId.Value);
            dto.SiteDisplayName = parent?.Name;
        }

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of BlogPosts
    /// </summary>
    public virtual async Task<PagedResultDto<BlogPostDto>> GetListAsync(BlogPostGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Dominus.BlogPost.BlogPost>, List<BlogPostDto>>(entities);
        var siteIds = entities
            .Where(x => x.SiteId != null)
            .Select(x => x.SiteId.Value)
            .Distinct()
            .ToList();

        if (siteIds.Any())
        {
            var parents = await _siteRepository.GetListAsync(x => siteIds.Contains(x.Id));
            var parentMap = parents.ToDictionary(x => x.Id, x => x.Name);

            foreach (var dto in dtoList.Where(x => x.SiteId != null))
            {
                if (parentMap.TryGetValue(dto.SiteId.Value, out var displayName))
                {
                    dto.SiteDisplayName = displayName;
                }
            }
        }

        return new PagedResultDto<BlogPostDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new BlogPost
    /// </summary>
    [Authorize(BlogPostPermissions.Create)]
    public virtual async Task<BlogPostDto> CreateAsync(CreateUpdateBlogPostDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateBlogPostDto, Dominus.BlogPost.BlogPost>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogPost.BlogPost, BlogPostDto>(entity);
    }

    /// <summary>
    /// Updates an existing BlogPost
    /// </summary>
    [Authorize(BlogPostPermissions.Update)]
    public virtual async Task<BlogPostDto> UpdateAsync(Guid id, CreateUpdateBlogPostDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Dominus.BlogPost.BlogPost), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Dominus.BlogPost.BlogPost, BlogPostDto>(entity);
    }

    /// <summary>
    /// Deletes a BlogPost
    /// </summary>
    [Authorize(BlogPostPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetBlogPostLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.Title
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Dominus.BlogPost.BlogPost> ApplyFilters(IQueryable<Dominus.BlogPost.BlogPost> queryable, BlogPostGetListInput input)
    {
        return queryable
            // ========== FK Filters ==========
            .WhereIf(input.SiteId != null, x => x.SiteId == input.SiteId)
            ;
    }
}
