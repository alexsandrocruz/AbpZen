using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.flwGradeHorarios.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.flwGradeHorarios;

/// <summary>
/// Application service for flwGradeHorarios entity
/// </summary>
[Authorize(flwGradeHorariosPermissions.Default)]
public class flwGradeHorariosAppService :
    LexusAppService,
    IflwGradeHorariosAppService
{
    private readonly IRepository<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid> _repository;

    public flwGradeHorariosAppService(
        IRepository<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single flwGradeHorarios by Id
    /// </summary>
    public virtual async Task<flwGradeHorariosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, flwGradeHorariosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of flwGradeHorarioses
    /// </summary>
    public virtual async Task<PagedResultDto<flwGradeHorariosDto>> GetListAsync(flwGradeHorariosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios>, List<flwGradeHorariosDto>>(entities);

        return new PagedResultDto<flwGradeHorariosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new flwGradeHorarios
    /// </summary>
    [Authorize(flwGradeHorariosPermissions.Create)]
    public virtual async Task<flwGradeHorariosDto> CreateAsync(CreateUpdateflwGradeHorariosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateflwGradeHorariosDto, Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, flwGradeHorariosDto>(entity);
    }

    /// <summary>
    /// Updates an existing flwGradeHorarios
    /// </summary>
    [Authorize(flwGradeHorariosPermissions.Update)]
    public virtual async Task<flwGradeHorariosDto> UpdateAsync(Guid id, CreateUpdateflwGradeHorariosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios, flwGradeHorariosDto>(entity);
    }

    /// <summary>
    /// Deletes a flwGradeHorarios
    /// </summary>
    [Authorize(flwGradeHorariosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetflwGradeHorariosLookupAsync()
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
    protected virtual IQueryable<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios> ApplyFilters(IQueryable<Sapienza.Lexus.flwGradeHorarios.flwGradeHorarios> queryable, flwGradeHorariosGetListInput input)
    {
        return queryable
            .WhereIf(input.idGrade != null, x => x.idGrade == input.idGrade)
            .WhereIf(input.idHistoricoTipo != null, x => x.idHistoricoTipo == input.idHistoricoTipo)
            .WhereIf(input.manhaHorarioInicial != null, x => x.manhaHorarioInicial == input.manhaHorarioInicial)
            .WhereIf(input.manhaIntervalo != null, x => x.manhaIntervalo == input.manhaIntervalo)
            .WhereIf(input.manhaQtde != null, x => x.manhaQtde == input.manhaQtde)
            .WhereIf(input.tardeHorarioInicial != null, x => x.tardeHorarioInicial == input.tardeHorarioInicial)
            .WhereIf(input.tardeIntervalo != null, x => x.tardeIntervalo == input.tardeIntervalo)
            .WhereIf(input.tardeQtde != null, x => x.tardeQtde == input.tardeQtde)
            .WhereIf(input.dom != null, x => x.dom == input.dom)
            .WhereIf(input.seg != null, x => x.seg == input.seg)
            .WhereIf(input.ter != null, x => x.ter == input.ter)
            .WhereIf(input.qua != null, x => x.qua == input.qua)
            .WhereIf(input.qui != null, x => x.qui == input.qui)
            .WhereIf(input.sex != null, x => x.sex == input.sex)
            .WhereIf(input.sab != null, x => x.sab == input.sab)
            // ========== FK Filters ==========
            ;
    }
}
