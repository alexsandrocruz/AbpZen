using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;

namespace Sapienza.Lexus.Web.Pages.advClientesAtualizacoes.ViewModels;

public class EditadvClientesAtualizacoesViewModel
{
    [Display(Name = "advClientesAtualizacoes:idAtualizacao")]
    public int? idAtualizacao { get; set; }
    [Required]
    [Display(Name = "advClientesAtualizacoes:idCliente")]
    public int idCliente { get; set; }
    [Display(Name = "advClientesAtualizacoes:campo")]
    public string? campo { get; set; }
    [Display(Name = "advClientesAtualizacoes:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [Display(Name = "advClientesAtualizacoes:dadoAnterior")]
    public string? dadoAnterior { get; set; }
    [Display(Name = "advClientesAtualizacoes:idUsuario")]
    public int? idUsuario { get; set; }

    // ========== Foreign Key Fields (1:N Relationships) ==========

    // ========== Child Collections (1:N Master-Detail) ==========
}
