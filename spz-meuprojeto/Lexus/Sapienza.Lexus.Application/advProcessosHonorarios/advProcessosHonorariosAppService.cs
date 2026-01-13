using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advProcessosHonorarios.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advProcessosHonorarios;

/// <summary>
/// Application service for advProcessosHonorarios entity
/// </summary>
[Authorize(advProcessosHonorariosPermissions.Default)]
public class advProcessosHonorariosAppService :
    LexusAppService,
    IadvProcessosHonorariosAppService
{
    private readonly IRepository<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, Guid> _repository;

    public advProcessosHonorariosAppService(
        IRepository<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advProcessosHonorarios by Id
    /// </summary>
    public virtual async Task<advProcessosHonorariosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, advProcessosHonorariosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advProcessosHonorarioses
    /// </summary>
    public virtual async Task<PagedResultDto<advProcessosHonorariosDto>> GetListAsync(advProcessosHonorariosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios>, List<advProcessosHonorariosDto>>(entities);

        return new PagedResultDto<advProcessosHonorariosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advProcessosHonorarios
    /// </summary>
    [Authorize(advProcessosHonorariosPermissions.Create)]
    public virtual async Task<advProcessosHonorariosDto> CreateAsync(CreateUpdateadvProcessosHonorariosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvProcessosHonorariosDto, Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, advProcessosHonorariosDto>(entity);
    }

    /// <summary>
    /// Updates an existing advProcessosHonorarios
    /// </summary>
    [Authorize(advProcessosHonorariosPermissions.Update)]
    public virtual async Task<advProcessosHonorariosDto> UpdateAsync(Guid id, CreateUpdateadvProcessosHonorariosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios, advProcessosHonorariosDto>(entity);
    }

    /// <summary>
    /// Deletes a advProcessosHonorarios
    /// </summary>
    [Authorize(advProcessosHonorariosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvProcessosHonorariosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.dataPrevistaClienteReceber
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios> ApplyFilters(IQueryable<Sapienza.Lexus.advProcessosHonorarios.advProcessosHonorarios> queryable, advProcessosHonorariosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.dataPrevistaClienteReceber.Contains(input.Filter) || x.rpv.Contains(input.Filter) || x.precatorio.Contains(input.Filter) || x.valorHonorariosTipo.Contains(input.Filter) || x.honorariosTextoFicha.Contains(input.Filter) || x.valorHonorariosDestaqueTipo.Contains(input.Filter) || x.dataPrevisaoHonorariosDestaque.Contains(input.Filter) || x.dataLiberacaoValorDeferido.Contains(input.Filter) || x.dataPrevisaoRepasseCliente.Contains(input.Filter) || x.formaRecebimento.Contains(input.Filter) || x.sucumbenciaAddData.Contains(input.Filter) || x.emitir.Contains(input.Filter) || x.bancarioCpf.Contains(input.Filter) || x.herdeirosTipoValor.Contains(input.Filter) || x.tarifaParcelas.Contains(input.Filter) || x.bancarioFavorecido.Contains(input.Filter) || x.bancarioTipoConta.Contains(input.Filter) || x.bancarioAgencia.Contains(input.Filter) || x.bancarioConta.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.alteradoPor.Contains(input.Filter) || x.valorHonorariosDestaqueTipoSomente.Contains(input.Filter) || x.dataPrevisaoHonorariosDestaqueSomente.Contains(input.Filter))
            .WhereIf(input.idHonorario != null, x => x.idHonorario == input.idHonorario)
            .WhereIf(input.idProcesso != null, x => x.idProcesso == input.idProcesso)
            .WhereIf(!input.dataPrevistaClienteReceber.IsNullOrWhiteSpace(), x => x.dataPrevistaClienteReceber.Contains(input.dataPrevistaClienteReceber))
            .WhereIf(!input.rpv.IsNullOrWhiteSpace(), x => x.rpv.Contains(input.rpv))
            .WhereIf(!input.precatorio.IsNullOrWhiteSpace(), x => x.precatorio.Contains(input.precatorio))
            .WhereIf(input.nrParcelasProcesso != null, x => x.nrParcelasProcesso == input.nrParcelasProcesso)
            .WhereIf(input.valorHonorarios != null, x => x.valorHonorarios == input.valorHonorarios)
            .WhereIf(!input.valorHonorariosTipo.IsNullOrWhiteSpace(), x => x.valorHonorariosTipo.Contains(input.valorHonorariosTipo))
            .WhereIf(!input.honorariosTextoFicha.IsNullOrWhiteSpace(), x => x.honorariosTextoFicha.Contains(input.honorariosTextoFicha))
            .WhereIf(input.valorHonorariosDestaque != null, x => x.valorHonorariosDestaque == input.valorHonorariosDestaque)
            .WhereIf(!input.valorHonorariosDestaqueTipo.IsNullOrWhiteSpace(), x => x.valorHonorariosDestaqueTipo.Contains(input.valorHonorariosDestaqueTipo))
            .WhereIf(!input.dataPrevisaoHonorariosDestaque.IsNullOrWhiteSpace(), x => x.dataPrevisaoHonorariosDestaque.Contains(input.dataPrevisaoHonorariosDestaque))
            .WhereIf(input.imposto != null, x => x.imposto == input.imposto)
            .WhereIf(input.complementoPositivo != null, x => x.complementoPositivo == input.complementoPositivo)
            .WhereIf(input.sucumbencia != null, x => x.sucumbencia == input.sucumbencia)
            .WhereIf(input.saldoDevedor != null, x => x.saldoDevedor == input.saldoDevedor)
            .WhereIf(input.valorDeferido != null, x => x.valorDeferido == input.valorDeferido)
            .WhereIf(!input.dataLiberacaoValorDeferido.IsNullOrWhiteSpace(), x => x.dataLiberacaoValorDeferido.Contains(input.dataLiberacaoValorDeferido))
            .WhereIf(input.idConta != null, x => x.idConta == input.idConta)
            .WhereIf(!input.dataPrevisaoRepasseCliente.IsNullOrWhiteSpace(), x => x.dataPrevisaoRepasseCliente.Contains(input.dataPrevisaoRepasseCliente))
            .WhereIf(input.idContaPagar != null, x => x.idContaPagar == input.idContaPagar)
            .WhereIf(!input.formaRecebimento.IsNullOrWhiteSpace(), x => x.formaRecebimento.Contains(input.formaRecebimento))
            .WhereIf(input.nrParcelasSomenteSucumbencia != null, x => x.nrParcelasSomenteSucumbencia == input.nrParcelasSomenteSucumbencia)
            .WhereIf(input.sucumbenciaAdd != null, x => x.sucumbenciaAdd == input.sucumbenciaAdd)
            .WhereIf(!input.sucumbenciaAddData.IsNullOrWhiteSpace(), x => x.sucumbenciaAddData.Contains(input.sucumbenciaAddData))
            .WhereIf(input.sucumbenciaAddIdBanco != null, x => x.sucumbenciaAddIdBanco == input.sucumbenciaAddIdBanco)
            .WhereIf(input.boleto != null, x => x.boleto == input.boleto)
            .WhereIf(!input.emitir.IsNullOrWhiteSpace(), x => x.emitir.Contains(input.emitir))
            .WhereIf(input.emitido != null, x => x.emitido == input.emitido)
            .WhereIf(input.nfComComplementoPositivo != null, x => x.nfComComplementoPositivo == input.nfComComplementoPositivo)
            .WhereIf(!input.bancarioCpf.IsNullOrWhiteSpace(), x => x.bancarioCpf.Contains(input.bancarioCpf))
            .WhereIf(!input.herdeirosTipoValor.IsNullOrWhiteSpace(), x => x.herdeirosTipoValor.Contains(input.herdeirosTipoValor))
            .WhereIf(input.bancarioPerc != null, x => x.bancarioPerc == input.bancarioPerc)
            .WhereIf(input.tarifa != null, x => x.tarifa == input.tarifa)
            .WhereIf(!input.tarifaParcelas.IsNullOrWhiteSpace(), x => x.tarifaParcelas.Contains(input.tarifaParcelas))
            .WhereIf(!input.bancarioFavorecido.IsNullOrWhiteSpace(), x => x.bancarioFavorecido.Contains(input.bancarioFavorecido))
            .WhereIf(input.bancarioBancoId != null, x => x.bancarioBancoId == input.bancarioBancoId)
            .WhereIf(!input.bancarioTipoConta.IsNullOrWhiteSpace(), x => x.bancarioTipoConta.Contains(input.bancarioTipoConta))
            .WhereIf(!input.bancarioAgencia.IsNullOrWhiteSpace(), x => x.bancarioAgencia.Contains(input.bancarioAgencia))
            .WhereIf(!input.bancarioConta.IsNullOrWhiteSpace(), x => x.bancarioConta.Contains(input.bancarioConta))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.alteradoPor.IsNullOrWhiteSpace(), x => x.alteradoPor.Contains(input.alteradoPor))
            .WhereIf(input.valorHonorariosDestaqueSomente != null, x => x.valorHonorariosDestaqueSomente == input.valorHonorariosDestaqueSomente)
            .WhereIf(!input.valorHonorariosDestaqueTipoSomente.IsNullOrWhiteSpace(), x => x.valorHonorariosDestaqueTipoSomente.Contains(input.valorHonorariosDestaqueTipoSomente))
            .WhereIf(!input.dataPrevisaoHonorariosDestaqueSomente.IsNullOrWhiteSpace(), x => x.dataPrevisaoHonorariosDestaqueSomente.Contains(input.dataPrevisaoHonorariosDestaqueSomente))
            .WhereIf(input.idBancoDestaqueSomente != null, x => x.idBancoDestaqueSomente == input.idBancoDestaqueSomente)
            // ========== FK Filters ==========
            ;
    }
}
