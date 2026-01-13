using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advCliLog.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advCliLog;

/// <summary>
/// Application service for advCliLog entity
/// </summary>
[Authorize(advCliLogPermissions.Default)]
public class advCliLogAppService :
    LexusAppService,
    IadvCliLogAppService
{
    private readonly IRepository<Sapienza.Lexus.advCliLog.advCliLog, Guid> _repository;

    public advCliLogAppService(
        IRepository<Sapienza.Lexus.advCliLog.advCliLog, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advCliLog by Id
    /// </summary>
    public virtual async Task<advCliLogDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advCliLog.advCliLog, advCliLogDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advCliLogs
    /// </summary>
    public virtual async Task<PagedResultDto<advCliLogDto>> GetListAsync(advCliLogGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advCliLog.advCliLog>, List<advCliLogDto>>(entities);

        return new PagedResultDto<advCliLogDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advCliLog
    /// </summary>
    [Authorize(advCliLogPermissions.Create)]
    public virtual async Task<advCliLogDto> CreateAsync(CreateUpdateadvCliLogDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvCliLogDto, Sapienza.Lexus.advCliLog.advCliLog>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliLog.advCliLog, advCliLogDto>(entity);
    }

    /// <summary>
    /// Updates an existing advCliLog
    /// </summary>
    [Authorize(advCliLogPermissions.Update)]
    public virtual async Task<advCliLogDto> UpdateAsync(Guid id, CreateUpdateadvCliLogDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advCliLog.advCliLog), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advCliLog.advCliLog, advCliLogDto>(entity);
    }

    /// <summary>
    /// Deletes a advCliLog
    /// </summary>
    [Authorize(advCliLogPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvCliLogLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.advCliLog.advCliLog> ApplyFilters(IQueryable<Sapienza.Lexus.advCliLog.advCliLog> queryable, advCliLogGetListInput input)
    {
        return queryable
            .WhereIf(input.idLog != null, x => x.idLog == input.idLog)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(input.acao != null, x => x.acao == input.acao)
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(input.idResponsavel != null, x => x.idResponsavel == input.idResponsavel)
            .WhereIf(input.dataAgendamento != null, x => x.dataAgendamento == input.dataAgendamento)
            .WhereIf(input.inssIdTipoBeneficio != null, x => x.inssIdTipoBeneficio == input.inssIdTipoBeneficio)
            // ========== FK Filters ==========
            ;
    }
}
