using System;
using Volo.Abp.Application.Dtos;

namespace Sapienza.EventoZen.Client.Dtos;

[Serializable]
public class ClientGetListInput : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public string? Name { get; set; }
    public ClientType? Type { get; set; }
    public string? Document { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Notes { get; set; }
    public bool? IsActive { get; set; }
    public string? LeadStatus { get; set; }
    public DateTime? FirstContactDate { get; set; }
    public DateTime? LastContactDate { get; set; }

    // ========== FK Filter Fields (Filter by parent entity) ==========
}
