using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientesINSS.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientesINSS;

/// <summary>
/// Application service for advClientesINSS entity
/// </summary>
[Authorize(advClientesINSSPermissions.Default)]
public class advClientesINSSAppService :
    LexusAppService,
    IadvClientesINSSAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientesINSS.advClientesINSS, Guid> _repository;

    public advClientesINSSAppService(
        IRepository<Sapienza.Lexus.advClientesINSS.advClientesINSS, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientesINSS by Id
    /// </summary>
    public virtual async Task<advClientesINSSDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientesINSS.advClientesINSS, advClientesINSSDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientesINSSes
    /// </summary>
    public virtual async Task<PagedResultDto<advClientesINSSDto>> GetListAsync(advClientesINSSGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientesINSS.advClientesINSS>, List<advClientesINSSDto>>(entities);

        return new PagedResultDto<advClientesINSSDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientesINSS
    /// </summary>
    [Authorize(advClientesINSSPermissions.Create)]
    public virtual async Task<advClientesINSSDto> CreateAsync(CreateUpdateadvClientesINSSDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientesINSSDto, Sapienza.Lexus.advClientesINSS.advClientesINSS>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesINSS.advClientesINSS, advClientesINSSDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientesINSS
    /// </summary>
    [Authorize(advClientesINSSPermissions.Update)]
    public virtual async Task<advClientesINSSDto> UpdateAsync(Guid id, CreateUpdateadvClientesINSSDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientesINSS.advClientesINSS), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientesINSS.advClientesINSS, advClientesINSSDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientesINSS
    /// </summary>
    [Authorize(advClientesINSSPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientesINSSLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.inssData
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientesINSS.advClientesINSS> ApplyFilters(IQueryable<Sapienza.Lexus.advClientesINSS.advClientesINSS> queryable, advClientesINSSGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.inssData.Contains(input.Filter) || x.inssResultado.Contains(input.Filter) || x.tsInclusao.Contains(input.Filter) || x.tsAlteracao.Contains(input.Filter) || x.inssProtocolo.Contains(input.Filter) || x.dataFinalizacao.Contains(input.Filter))
            .WhereIf(input.idInssAgendado != null, x => x.idInssAgendado == input.idInssAgendado)
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(input.inssAgendado != null, x => x.inssAgendado == input.inssAgendado)
            .WhereIf(!input.inssData.IsNullOrWhiteSpace(), x => x.inssData.Contains(input.inssData))
            .WhereIf(input.inssIdTipoBeneficio != null, x => x.inssIdTipoBeneficio == input.inssIdTipoBeneficio)
            .WhereIf(input.inssIdPosto != null, x => x.inssIdPosto == input.inssIdPosto)
            .WhereIf(!input.inssResultado.IsNullOrWhiteSpace(), x => x.inssResultado.Contains(input.inssResultado))
            .WhereIf(!input.tsInclusao.IsNullOrWhiteSpace(), x => x.tsInclusao.Contains(input.tsInclusao))
            .WhereIf(!input.tsAlteracao.IsNullOrWhiteSpace(), x => x.tsAlteracao.Contains(input.tsAlteracao))
            .WhereIf(input.inssResultadoIndicadorOculto != null, x => x.inssResultadoIndicadorOculto == input.inssResultadoIndicadorOculto)
            .WhereIf(input.inssResponsavel != null, x => x.inssResponsavel == input.inssResponsavel)
            .WhereIf(!input.inssProtocolo.IsNullOrWhiteSpace(), x => x.inssProtocolo.Contains(input.inssProtocolo))
            .WhereIf(input.inssIdUsuarioInclusao != null, x => x.inssIdUsuarioInclusao == input.inssIdUsuarioInclusao)
            .WhereIf(input.idStatus != null, x => x.idStatus == input.idStatus)
            .WhereIf(!input.dataFinalizacao.IsNullOrWhiteSpace(), x => x.dataFinalizacao.Contains(input.dataFinalizacao))
            // ========== FK Filters ==========
            ;
    }
}
