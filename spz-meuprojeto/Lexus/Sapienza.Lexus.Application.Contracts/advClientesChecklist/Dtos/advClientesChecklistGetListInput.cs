using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.Lexus.advClientesChecklist.Dtos;

[Serializable]
public class advClientesChecklistGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public int? idClienteChecklist { get; set; }
    public int? idCliente { get; set; }
    public int? idTipoArquivo { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
