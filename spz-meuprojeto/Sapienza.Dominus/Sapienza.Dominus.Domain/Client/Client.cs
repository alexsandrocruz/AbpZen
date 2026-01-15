// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Client;

/// <summary>
/// Client entity
/// </summary>
public class Client : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; }
    public string ClientType { get; set; }
    public string CompanyName { get; set; }
    public string CNPJ { get; set; }
    public string CPF { get; set; }
    public DateTime BirthDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string Status { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========

    // ========== Navigation Properties ==========

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.ClientContact.ClientContact> ClientContacts { get; set; } = new List<Dominus.ClientContact.ClientContact>();
    public virtual ICollection<Dominus.ClientMessage.ClientMessage> ClientMessages { get; set; } = new List<Dominus.ClientMessage.ClientMessage>();
    public virtual ICollection<Dominus.Project.Project> Projects { get; set; } = new List<Dominus.Project.Project>();
    public virtual ICollection<Dominus.Transaction.Transaction> Transactions { get; set; } = new List<Dominus.Transaction.Transaction>();
    public virtual ICollection<Dominus.Proposal.Proposal> Proposals { get; set; } = new List<Dominus.Proposal.Proposal>();
    public virtual ICollection<Dominus.Contract.Contract> Contracts { get; set; } = new List<Dominus.Contract.Contract>();
    public virtual ICollection<Dominus.Booking.Booking> Bookings { get; set; } = new List<Dominus.Booking.Booking>();
    public virtual ICollection<Dominus.Conversation.Conversation> Conversations { get; set; } = new List<Dominus.Conversation.Conversation>();

    protected Client()
    {
        // Required by EF Core
    }

    public Client(Guid id) : base(id)
    {
    }
}
