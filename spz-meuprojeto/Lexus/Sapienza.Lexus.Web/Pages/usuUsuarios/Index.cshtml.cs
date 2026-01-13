using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using Sapienza.Lexus.usuUsuarios;
using Sapienza.Lexus.usuUsuarios.Dtos;

namespace Sapienza.Lexus.Web.Pages.usuUsuarios;

public class IndexModel : Sapienza.LexusPageModel
{
    public usuUsuariosFilterInput usuUsuariosFilter { get; set; }
    
    private readonly IusuUsuariosAppService _usuUsuariosAppService;

    public IndexModel(IusuUsuariosAppService usuUsuariosAppService)
    {
        _usuUsuariosAppService = usuUsuariosAppService;
    }

    public virtual async Task OnGetAsync()
    {
        await Task.CompletedTask;
    }

    public virtual async Task<JsonResult> OnGetListAsync(usuUsuariosGetListInput input)
    {
        var result = await _usuUsuariosAppService.GetListAsync(input);
        return new JsonResult(result);
    }

    public virtual async Task<IActionResult> OnPostDeleteAsync(Guid id)
    {
        await _usuUsuariosAppService.DeleteAsync(id);
        return new NoContentResult();
    }
}

public class usuUsuariosFilterInput
{
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:idUsuario")]
    public int? idUsuario { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:nome")]
    public string? nome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:sobrenome")]
    public string? sobrenome { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:idArea")]
    public int? idArea { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:idCargo")]
    public int? idCargo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:login")]
    public string? login { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:senha")]
    public string? senha { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:diaNascimento")]
    public int? diaNascimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:mesNascimento")]
    public int? mesNascimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:anoNascimento")]
    public int? anoNascimento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:email")]
    public string? email { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:telCelular")]
    public string? telCelular { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:telFixo")]
    public string? telFixo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:endereco")]
    public string? endereco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:numero")]
    public string? numero { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:complemento")]
    public string? complemento { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:bairro")]
    public string? bairro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:cep")]
    public string? cep { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:estado")]
    public string? estado { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:cidade")]
    public string? cidade { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:cpf")]
    public string? cpf { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:banco")]
    public string? banco { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:agencia")]
    public string? agencia { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:conta")]
    public string? conta { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:foto")]
    public string? foto { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:ativo")]
    public bool? ativo { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:tsInclusao")]
    public DateTime? tsInclusao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:tsAlteracao")]
    public DateTime? tsAlteracao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:cor")]
    public string? cor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:dashboardInicial")]
    public string? dashboardInicial { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:tokenPhoneApp")]
    public string? tokenPhoneApp { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:estadoCivil")]
    public string? estadoCivil { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:nrFilhos")]
    public int? nrFilhos { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:idadeFilhoMenor")]
    public int? idadeFilhoMenor { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:formacaoAcademica")]
    public string? formacaoAcademica { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:regiao")]
    public string? regiao { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:idSuperior")]
    public int? idSuperior { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:master")]
    public bool? master { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:mediaConsumoLitro")]
    public int? mediaConsumoLitro { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:distanciasIguais")]
    public string? distanciasIguais { get; set; }
    [FormControlSize(AbpFormControlSize.Small)]
    [Display(Name = "usuUsuarios:chaveChamados")]
    public string? chaveChamados { get; set; }
}
