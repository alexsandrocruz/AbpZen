using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.finPlanoContas.Dtos;

[Serializable]
public class finPlanoContasGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idPlanoConta { get; set; }
    public int? idGrupo { get; set; }
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

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
