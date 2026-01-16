using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sapienza.Lexus.finCentrosCusto.Dtos;

[Serializable]
public class CreateUpdatefinCentrosCustoDto
{
    /// <summary>
    /// Id for Master-Detail reconciliation (empty = new item)
    /// </summary>
    public Guid Id { get; set; }
    public int? idCentroCusto { get; set; }
    public int? idUnidade { get; set; }
    public string titulo { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? padrao { get; set; }
    public double? porcentagemRateio { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========
    public Guid? finLancamentosId { get; set; }

    // ========== Child Collections (1:N Master-Detail) ==========
}
