// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.usuUsuarios;

/// <summary>
/// usuUsuarios entity
/// </summary>
public class usuUsuarios : FullAuditedAggregateRoot<Guid>
{
    public int? idUsuario { get; set; }
    public string? nome { get; set; }
    public string? sobrenome { get; set; }
    public int idArea { get; set; }
    public int idCargo { get; set; }
    public string? login { get; set; }
    public string? senha { get; set; }
    public int? diaNascimento { get; set; }
    public int? mesNascimento { get; set; }
    public int? anoNascimento { get; set; }
    public string? email { get; set; }
    public string? telCelular { get; set; }
    public string? telFixo { get; set; }
    public string? endereco { get; set; }
    public string? numero { get; set; }
    public string? complemento { get; set; }
    public string? bairro { get; set; }
    public string? cep { get; set; }
    public string? estado { get; set; }
    public string? cidade { get; set; }
    public string? cpf { get; set; }
    public string? banco { get; set; }
    public string? agencia { get; set; }
    public string? conta { get; set; }
    public string? foto { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public string? cor { get; set; }
    public string? dashboardInicial { get; set; }
    public string? tokenPhoneApp { get; set; }
    public string? estadoCivil { get; set; }
    public int? nrFilhos { get; set; }
    public int? idadeFilhoMenor { get; set; }
    public string? formacaoAcademica { get; set; }
    public string? regiao { get; set; }
    public int? idSuperior { get; set; }
    public bool? master { get; set; }
    public int? mediaConsumoLitro { get; set; }
    public string? distanciasIguais { get; set; }
    public string? chaveChamados { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========

    protected usuUsuarios()
    {
        // Required by EF Core
    }

    public usuUsuarios(Guid id) : base(id)
    {
    }
}
