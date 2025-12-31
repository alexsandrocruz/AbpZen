using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Volo.Abp.Gdpr;

public class GdprRequestInput : PagedAndSortedResultRequestDto
{
    [Required]
    public Guid UserId { get; set; }
}