using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sapienza.Lexus.Permissions;
using Sapienza.Lexus.usuUsuarios.Dtos;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Sapienza.Lexus.usuUsuarios;

/// <summary>
/// Application service for usuUsuarios entity
/// </summary>
[Authorize(usuUsuariosPermissions.Default)]
public class usuUsuariosAppService :
    LexusAppService,
    IusuUsuariosAppService
{
    private readonly IRepository<Sapienza.Lexus.usuUsuarios.usuUsuarios, Guid> _repository;

    public usuUsuariosAppService(
        IRepository<Sapienza.Lexus.usuUsuarios.usuUsuarios, Guid> repository
    )
    {
        _repository = repository;
    }

    /// <summary>
    /// Gets a single usuUsuarios by Id
    /// </summary>
    public virtual async Task<usuUsuariosDto> GetAsync(Guid id)
    {
        var entity = await _repository.GetAsync(id);
        var dto = ObjectMapper.Map<Sapienza.Lexus.usuUsuarios.usuUsuarios, usuUsuariosDto>(entity);

        return dto;
    }

    /// <summary>
    /// Gets a paged and filtered list of usuUsuarioses
    /// </summary>
    public virtual async Task<PagedResultDto<usuUsuariosDto>> GetListAsync(usuUsuariosGetListInput input)
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
        var dtoList = ObjectMapper.Map<List<Sapienza.Lexus.usuUsuarios.usuUsuarios>, List<usuUsuariosDto>>(entities);

        return new PagedResultDto<usuUsuariosDto>(
            totalCount,
            dtoList
        );
    }

    /// <summary>
    /// Creates a new usuUsuarios
    /// </summary>
    [Authorize(usuUsuariosPermissions.Create)]
    public virtual async Task<usuUsuariosDto> CreateAsync(CreateUpdateusuUsuariosDto input)
    {
        var entity = ObjectMapper.Map<CreateUpdateusuUsuariosDto, Sapienza.Lexus.usuUsuarios.usuUsuarios>(input);

        await _repository.InsertAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuUsuarios.usuUsuarios, usuUsuariosDto>(entity);
    }

    /// <summary>
    /// Updates an existing usuUsuarios
    /// </summary>
    [Authorize(usuUsuariosPermissions.Update)]
    public virtual async Task<usuUsuariosDto> UpdateAsync(Guid id, CreateUpdateusuUsuariosDto input)
    {
        var entity = await _repository.GetAsync(id);
        if (entity == null)
        {
             throw new Volo.Abp.Domain.Entities.EntityNotFoundException(typeof(Sapienza.Lexus.usuUsuarios.usuUsuarios), id);
        }

        ObjectMapper.Map(input, entity);

        await _repository.UpdateAsync(entity, autoSave: true);

        return ObjectMapper.Map<Sapienza.Lexus.usuUsuarios.usuUsuarios, usuUsuariosDto>(entity);
    }

    /// <summary>
    /// Deletes a usuUsuarios
    /// </summary>
    [Authorize(usuUsuariosPermissions.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public virtual async Task<ListResultDto<LookupDto<Guid>>> GetusuUsuariosLookupAsync()
    {
        var entities = await _repository.GetListAsync();return new ListResultDto<LookupDto<Guid>>(
            entities.Select(x => new LookupDto<Guid>
            {
                Id = x.Id,
                DisplayName = x.nome
            }).ToList()
        );
    }

    /// <summary>
    /// Applies filters to the queryable based on input parameters
    /// </summary>
    protected virtual IQueryable<Sapienza.Lexus.usuUsuarios.usuUsuarios> ApplyFilters(IQueryable<Sapienza.Lexus.usuUsuarios.usuUsuarios> queryable, usuUsuariosGetListInput input)
    {
        return queryable
            .WhereIf(!input.Filter.IsNullOrWhiteSpace(), x =>x.nome.Contains(input.Filter) || x.sobrenome.Contains(input.Filter) || x.login.Contains(input.Filter) || x.senha.Contains(input.Filter) || x.email.Contains(input.Filter) || x.telCelular.Contains(input.Filter) || x.telFixo.Contains(input.Filter) || x.endereco.Contains(input.Filter) || x.numero.Contains(input.Filter) || x.complemento.Contains(input.Filter) || x.bairro.Contains(input.Filter) || x.cep.Contains(input.Filter) || x.estado.Contains(input.Filter) || x.cidade.Contains(input.Filter) || x.cpf.Contains(input.Filter) || x.banco.Contains(input.Filter) || x.agencia.Contains(input.Filter) || x.conta.Contains(input.Filter) || x.foto.Contains(input.Filter) || x.cor.Contains(input.Filter) || x.dashboardInicial.Contains(input.Filter) || x.tokenPhoneApp.Contains(input.Filter) || x.estadoCivil.Contains(input.Filter) || x.formacaoAcademica.Contains(input.Filter) || x.regiao.Contains(input.Filter) || x.distanciasIguais.Contains(input.Filter) || x.chaveChamados.Contains(input.Filter))
            .WhereIf(input.idUsuario != null, x => x.idUsuario == input.idUsuario)
            .WhereIf(!input.nome.IsNullOrWhiteSpace(), x => x.nome.Contains(input.nome))
            .WhereIf(!input.sobrenome.IsNullOrWhiteSpace(), x => x.sobrenome.Contains(input.sobrenome))
            .WhereIf(input.idArea != null, x => x.idArea == input.idArea)
            .WhereIf(input.idCargo != null, x => x.idCargo == input.idCargo)
            .WhereIf(!input.login.IsNullOrWhiteSpace(), x => x.login.Contains(input.login))
            .WhereIf(!input.senha.IsNullOrWhiteSpace(), x => x.senha.Contains(input.senha))
            .WhereIf(input.diaNascimento != null, x => x.diaNascimento == input.diaNascimento)
            .WhereIf(input.mesNascimento != null, x => x.mesNascimento == input.mesNascimento)
            .WhereIf(input.anoNascimento != null, x => x.anoNascimento == input.anoNascimento)
            .WhereIf(!input.email.IsNullOrWhiteSpace(), x => x.email.Contains(input.email))
            .WhereIf(!input.telCelular.IsNullOrWhiteSpace(), x => x.telCelular.Contains(input.telCelular))
            .WhereIf(!input.telFixo.IsNullOrWhiteSpace(), x => x.telFixo.Contains(input.telFixo))
            .WhereIf(!input.endereco.IsNullOrWhiteSpace(), x => x.endereco.Contains(input.endereco))
            .WhereIf(!input.numero.IsNullOrWhiteSpace(), x => x.numero.Contains(input.numero))
            .WhereIf(!input.complemento.IsNullOrWhiteSpace(), x => x.complemento.Contains(input.complemento))
            .WhereIf(!input.bairro.IsNullOrWhiteSpace(), x => x.bairro.Contains(input.bairro))
            .WhereIf(!input.cep.IsNullOrWhiteSpace(), x => x.cep.Contains(input.cep))
            .WhereIf(!input.estado.IsNullOrWhiteSpace(), x => x.estado.Contains(input.estado))
            .WhereIf(!input.cidade.IsNullOrWhiteSpace(), x => x.cidade.Contains(input.cidade))
            .WhereIf(!input.cpf.IsNullOrWhiteSpace(), x => x.cpf.Contains(input.cpf))
            .WhereIf(!input.banco.IsNullOrWhiteSpace(), x => x.banco.Contains(input.banco))
            .WhereIf(!input.agencia.IsNullOrWhiteSpace(), x => x.agencia.Contains(input.agencia))
            .WhereIf(!input.conta.IsNullOrWhiteSpace(), x => x.conta.Contains(input.conta))
            .WhereIf(!input.foto.IsNullOrWhiteSpace(), x => x.foto.Contains(input.foto))
            .WhereIf(input.ativo != null, x => x.ativo == input.ativo)
            .WhereIf(input.tsInclusao != null, x => x.tsInclusao == input.tsInclusao)
            .WhereIf(input.tsAlteracao != null, x => x.tsAlteracao == input.tsAlteracao)
            .WhereIf(!input.cor.IsNullOrWhiteSpace(), x => x.cor.Contains(input.cor))
            .WhereIf(!input.dashboardInicial.IsNullOrWhiteSpace(), x => x.dashboardInicial.Contains(input.dashboardInicial))
            .WhereIf(!input.tokenPhoneApp.IsNullOrWhiteSpace(), x => x.tokenPhoneApp.Contains(input.tokenPhoneApp))
            .WhereIf(!input.estadoCivil.IsNullOrWhiteSpace(), x => x.estadoCivil.Contains(input.estadoCivil))
            .WhereIf(input.nrFilhos != null, x => x.nrFilhos == input.nrFilhos)
            .WhereIf(input.idadeFilhoMenor != null, x => x.idadeFilhoMenor == input.idadeFilhoMenor)
            .WhereIf(!input.formacaoAcademica.IsNullOrWhiteSpace(), x => x.formacaoAcademica.Contains(input.formacaoAcademica))
            .WhereIf(!input.regiao.IsNullOrWhiteSpace(), x => x.regiao.Contains(input.regiao))
            .WhereIf(input.idSuperior != null, x => x.idSuperior == input.idSuperior)
            .WhereIf(input.master != null, x => x.master == input.master)
            .WhereIf(input.mediaConsumoLitro != null, x => x.mediaConsumoLitro == input.mediaConsumoLitro)
            .WhereIf(!input.distanciasIguais.IsNullOrWhiteSpace(), x => x.distanciasIguais.Contains(input.distanciasIguais))
            .WhereIf(!input.chaveChamados.IsNullOrWhiteSpace(), x => x.chaveChamados.Contains(input.chaveChamados))
            // ========== FK Filters ==========
            ;
    }
}
