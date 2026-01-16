// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Lexus.finPlanoContas;

/// <summary>
/// finPlanoContas entity
/// </summary>
public class finPlanoContas : FullAuditedAggregateRoot<Guid>
{
    public int? idPlanoConta { get; set; }
    public int idGrupo { get; set; }
    public string? titulo { get; set; }
    public string? codigo { get; set; }
    public bool? pagamentoSempreLiberado { get; set; }
    public bool? permiteLancamentoQuitado { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padraoVendas { get; set; }
    public bool? antecipaVencimento { get; set; }
    public bool? terceiroNivel { get; set; }
    public bool? criarPeloFinanceiro { get; set; }
    public bool? valoresRestritos { get; set; }
    public bool? naoAbatePagtoDoSaldoDoCliente { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? finLancamentosId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Sapienza.Lexus.finLancamentos.finLancamentos? finPlanoContasNav { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos> finPlanoContases { get; set; } = new List<Sapienza.Lexus.finPlanoContasGrupos.finPlanoContasGrupos>();

    protected finPlanoContas()
    {
        // Required by EF Core
    }

    public finPlanoContas(Guid id) : base(id)
    {
    }
}
