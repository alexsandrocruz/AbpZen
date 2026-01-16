using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPlanoContas.Dtos;

[Serializable]
public class finPlanoContasDto : FullAuditedEntityDto<Guid>
{
    public int? idPlanoConta { get; set; }
    public int idGrupo { get; set; }
    public string titulo { get; set; }
    public string codigo { get; set; }
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

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? finLancamentosId { get; set; }
    public string? finLancamentosDisplayName { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
