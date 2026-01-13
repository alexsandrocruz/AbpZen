using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advFornecedores.Dtos;

[Serializable]
public class advFornecedoresDto : FullAuditedEntityDto<Guid>
{
    public int? idFornecedor { get; set; }
    public string apelido { get; set; }
    public string nome { get; set; }
    public string email { get; set; }
    public string telCelular { get; set; }
    public string telCelularObs { get; set; }
    public string telFixo { get; set; }
    public string telFixoObs { get; set; }
    public string endereco { get; set; }
    public string numero { get; set; }
    public string complemento { get; set; }
    public string bairro { get; set; }
    public string cep { get; set; }
    public string estado { get; set; }
    public string cidade { get; set; }
    public string observacoes { get; set; }
    public bool? ativo { get; set; }
    public DateTime? tsInclusao { get; set; }
    public DateTime? tsAlteracao { get; set; }
    public bool? parceiroEmProcesso { get; set; }
    public double? parceiroEmProcessoPerc { get; set; }
    public int? idProfissional { get; set; }
    public string foto { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
