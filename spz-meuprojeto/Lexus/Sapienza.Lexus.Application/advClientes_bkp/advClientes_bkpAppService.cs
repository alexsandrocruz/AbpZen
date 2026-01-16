using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.advClientes_bkp.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.advClientes_bkp;

/// <summary>
/// Application service for advClientes_bkp entity
/// </summary>
[Authorize(advClientes_bkpPermissions.Default)]
public class advClientes_bkpAppService :
    LexusAppService,
    IadvClientes_bkpAppService
{
    private readonly IRepository<Sapienza.Lexus.advClientes_bkp.advClientes_bkp, Guid> _repository;

    public advClientes_bkpAppService(
        IRepository<Sapienza.Lexus.advClientes_bkp.advClientes_bkp, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single advClientes_bkp by Id
    /// </summary>
    public virtual async Task<advClientes_bkpDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.advClientes_bkp.advClientes_bkp, advClientes_bkpDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of advClientes_bkps
    /// </summary>
    public virtual async Task<PagedResultDto<advClientes_bkpDto>> GetListAsync(advClientes_bkpGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.advClientes_bkp.advClientes_bkp>, List<advClientes_bkpDto>>(entities);

        return new PagedResultDto<advClientes_bkpDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new advClientes_bkp
    /// </summary>
    [Authorize(advClientes_bkpPermissions.Create)]
    public virtual async Task<advClientes_bkpDto> CreateAsync(CreateUpdateadvClientes_bkpDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateadvClientes_bkpDto, Sapienza.Lexus.advClientes_bkp.advClientes_bkp>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientes_bkp.advClientes_bkp, advClientes_bkpDto>(entity);
    }

    /// <summary>
    /// Updates an existing advClientes_bkp
    /// </summary>
    [Authorize(advClientes_bkpPermissions.Update)]
    public virtual async Task<advClientes_bkpDto> UpdateAsync(Guid id, CreateUpdateadvClientes_bkpDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.advClientes_bkp.advClientes_bkp), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.advClientes_bkp.advClientes_bkp, advClientes_bkpDto>(entity);
    }

    /// <summary>
    /// Deletes a advClientes_bkp
    /// </summary>
    [Authorize(advClientes_bkpPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetadvClientes_bkpLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.apelido
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.advClientes_bkp.advClientes_bkp> ApplyFilters(IQueryable<Sapienza.Lexus.advClientes_bkp.advClientes_bkp> queryable, advClientes_bkpGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.apelido.Contains(input.Filter) || x.nome.Contains(input.Filter) || x.email.Contains(input.Filter) || x.telCelular.Contains(input.Filter) || x.telCelularObs.Contains(input.Filter) || x.telFixo.Contains(input.Filter) || x.telFixoObs.Contains(input.Filter) || x.dataNascimento.Contains(input.Filter) || x.cpf.Contains(input.Filter) || x.rg.Contains(input.Filter) || x.ctps.Contains(input.Filter) || x.endereco.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.complemento.Contains(input.Filter) || x.bairro.Contains(input.Filter) || x.cep.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.dataIngresso.Contains(input.Filter) || x.observacoes.Contains(input.Filter) || x.naturalEstado.Contains(input.Filter) || x.naturalCidade.Contains(input.Filter) || x.nomeDaMae.Contains(input.Filter) || x.dibData.Contains(input.Filter) || x.telCelular2.Contains(input.Filter) || x.telCelular2Obs.Contains(input.Filter) || x.telFixo2.Contains(input.Filter) || x.telFixo2Obs.Contains(input.Filter) || x.cnpj.Contains(input.Filter) || x.ie.Contains(input.Filter) || x.incluidoPor.Contains(input.Filter) || x.inssData.Contains(input.Filter) || x.inssResultado.Contains(input.Filter) || x.pastaFTP.Contains(input.Filter) || x.inssProtocolo.Contains(input.Filter) || x.foto.Contains(input.Filter) || x.followBloqueadoAte.Contains(input.Filter) || x.senhaINSSDigital.Contains(input.Filter) || x.instagram.Contains(input.Filter) || x.rgOrgaoExp.Contains(input.Filter) || x.nacionalidade.Contains(input.Filter) || x.estadocivil.Contains(input.Filter) || x.dcbData.Contains(input.Filter))
            .WhereIf(input.idCliente != null, x => x.idCliente == input.idCliente)
            .WhereIf(!input.apelido.IsNullOrWhiteSpace(), x => x.apelido.Contains(input.apelido))
            .WhereIf(input.idGrupo != null, x => x.idGrupo == input.idGrupo)
            .WhereIf(input.idSituacao != null, x => x.idSituacao == input.idSituacao)
            .WhereIf(!input.nome.IsNullOrWhiteSpace(), x => x.nome.Contains(input.nome))
            .WhereIf(!input.email.IsNullOrWhiteSpace(), x => x.email.Contains(input.email))
            .WhereIf(!input.telCelular.IsNullOrWhiteSpace(), x => x.telCelular.Contains(input.telCelular))
            .WhereIf(!input.telCelularObs.IsNullOrWhiteSpace(), x => x.telCelularObs.Contains(input.telCelularObs))
            .WhereIf(!input.telFixo.IsNullOrWhiteSpace(), x => x.telFixo.Contains(input.telFixo))
            .WhereIf(!input.telFixoObs.IsNullOrWhiteSpace(), x => x.telFixoObs.Contains(input.telFixoObs))
            .WhereIf(!input.dataNascimento.IsNullOrWhiteSpace(), x => x.dataNascimento.Contains(input.dataNascimento))
            .WhereIf(!input.cpf.IsNullOrWhiteSpace(), x => x.cpf.Contains(input.cpf))
            .WhereIf(!input.rg.IsNullOrWhiteSpace(), x => x.rg.Contains(input.rg))
            .WhereIf(!input.ctps.IsNullOrWhiteSpace(), x => x.ctps.Contains(input.ctps))
            .WhereIf(!input.endereco.IsNullOrWhiteSpace(), x => x.endereco.Contains(input.endereco))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.complemento.IsNullOrWhiteSpace(), x => x.complemento.Contains(input.complemento))
            .WhereIf(!input.bairro.IsNullOrWhiteSpace(), x => x.bairro.Contains(input.bairro))
            .WhereIf(!input.cep.IsNullOrWhiteSpace(), x => x.cep.Contains(input.cep))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(!input.dataIngresso.IsNullOrWhiteSpace(), x => x.dataIngresso.Contains(input.dataIngresso))
            .WhereIf(!input.observacoes.IsNullOrWhiteSpace(), x => x.observacoes.Contains(input.observacoes))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.naturalEstado.IsNullOrWhiteSpace(), x => x.naturalEstado.Contains(input.naturalEstado))
            .WhereIf(!input.naturalCidade.IsNullOrWhiteSpace(), x => x.naturalCidade.Contains(input.naturalCidade))
            .WhereIf(!input.nomeDaMae.IsNullOrWhiteSpace(), x => x.nomeDaMae.Contains(input.nomeDaMae))
            .WhereIf(input.dib != null, x => x.dib == input.dib)
            .WhereIf(!input.dibData.IsNullOrWhiteSpace(), x => x.dibData.Contains(input.dibData))
            .WhereIf(input.dibIdTipoBeneficio != null, x => x.dibIdTipoBeneficio == input.dibIdTipoBeneficio)
            .WhereIf(input.idCargo != null, x => x.idCargo == input.idCargo)
            .WhereIf(!input.telCelular2.IsNullOrWhiteSpace(), x => x.telCelular2.Contains(input.telCelular2))
            .WhereIf(!input.telCelular2Obs.IsNullOrWhiteSpace(), x => x.telCelular2Obs.Contains(input.telCelular2Obs))
            .WhereIf(!input.telFixo2.IsNullOrWhiteSpace(), x => x.telFixo2.Contains(input.telFixo2))
            .WhereIf(!input.telFixo2Obs.IsNullOrWhiteSpace(), x => x.telFixo2Obs.Contains(input.telFixo2Obs))
            .WhereIf(!input.cnpj.IsNullOrWhiteSpace(), x => x.cnpj.Contains(input.cnpj))
            .WhereIf(!input.ie.IsNullOrWhiteSpace(), x => x.ie.Contains(input.ie))
            .WhereIf(input.idFornecedor != null, x => x.idFornecedor == input.idFornecedor)
            .WhereIf(!input.incluidoPor.IsNullOrWhiteSpace(), x => x.incluidoPor.Contains(input.incluidoPor))
            .WhereIf(input.inssAgendado != null, x => x.inssAgendado == input.inssAgendado)
            .WhereIf(!input.inssData.IsNullOrWhiteSpace(), x => x.inssData.Contains(input.inssData))
            .WhereIf(input.inssIdTipoBeneficio != null, x => x.inssIdTipoBeneficio == input.inssIdTipoBeneficio)
            .WhereIf(input.inssIdPosto != null, x => x.inssIdPosto == input.inssIdPosto)
            .WhereIf(!input.inssResultado.IsNullOrWhiteSpace(), x => x.inssResultado.Contains(input.inssResultado))
            .WhereIf(input.prospect != null, x => x.prospect == input.prospect)
            .WhereIf(input.idLocalAtendido != null, x => x.idLocalAtendido == input.idLocalAtendido)
            .WhereIf(input.whatsapp != null, x => x.whatsapp == input.whatsapp)
            .WhereIf(!input.pastaFTP.IsNullOrWhiteSpace(), x => x.pastaFTP.Contains(input.pastaFTP))
            .WhereIf(input.inssResponsavel != null, x => x.inssResponsavel == input.inssResponsavel)
            .WhereIf(input.responsavelPendencia != null, x => x.responsavelPendencia == input.responsavelPendencia)
            .WhereIf(input.comoChegou != null, x => x.comoChegou == input.comoChegou)
            .WhereIf(!input.inssProtocolo.IsNullOrWhiteSpace(), x => x.inssProtocolo.Contains(input.inssProtocolo))
            .WhereIf(input.inssTsInclusao != null, x => x.inssTsInclusao == input.inssTsInclusao)
            .WhereIf(input.inssIdUsuarioInclusao != null, x => x.inssIdUsuarioInclusao == input.inssIdUsuarioInclusao)
            .WhereIf(!input.foto.IsNullOrWhiteSpace(), x => x.foto.Contains(input.foto))
            .WhereIf(!input.followBloqueadoAte.IsNullOrWhiteSpace(), x => x.followBloqueadoAte.Contains(input.followBloqueadoAte))
            .WhereIf(input.falecido != null, x => x.falecido == input.falecido)
            .WhereIf(!input.senhaINSSDigital.IsNullOrWhiteSpace(), x => x.senhaINSSDigital.Contains(input.senhaINSSDigital))
            .WhereIf(input.idPrioridade != null, x => x.idPrioridade == input.idPrioridade)
            .WhereIf(!input.instagram.IsNullOrWhiteSpace(), x => x.instagram.Contains(input.instagram))
            .WhereIf(!input.rgOrgaoExp.IsNullOrWhiteSpace(), x => x.rgOrgaoExp.Contains(input.rgOrgaoExp))
            .WhereIf(!input.nacionalidade.IsNullOrWhiteSpace(), x => x.nacionalidade.Contains(input.nacionalidade))
            .WhereIf(!input.estadocivil.IsNullOrWhiteSpace(), x => x.estadocivil.Contains(input.estadocivil))
            .WhereIf(input.dcb != null, x => x.dcb == input.dcb)
            .WhereIf(!input.dcbData.IsNullOrWhiteSpace(), x => x.dcbData.Contains(input.dcbData))
            // ========== FK Filters ==========
            ;
    }
}
