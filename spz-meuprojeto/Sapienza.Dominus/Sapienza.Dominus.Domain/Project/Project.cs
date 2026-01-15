// Generated with Fixed Generator
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace Sapienza.Dominus.Project;

/// <summary>
/// Project entity
/// </summary>
public class Project : FullAuditedAggregateRoot<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; }
    public string Status { get; set; }
    public double Budget { get; set; }
    public DateTime DueDate { get; set; }

    // ========== Foreign Key Properties (1:N - This entity is the "Many" side) ==========
    public Guid? ClientId { get; set; }

    // ========== Navigation Properties ==========
    public virtual Dominus.Client.Client? Client { get; set; }

    // ========== Collection Navigation Properties (1:N - This entity is the "One" side) ==========
    public virtual ICollection<Dominus.Task.Task> Tasks { get; set; } = new List<Dominus.Task.Task>();
    public virtual ICollection<Dominus.TimeEntry.TimeEntry> TimeEntries { get; set; } = new List<Dominus.TimeEntry.TimeEntry>();
    public virtual ICollection<Dominus.ProjectResponsible.ProjectResponsible> ProjectResponsibles { get; set; } = new List<Dominus.ProjectResponsible.ProjectResponsible>();
    public virtual ICollection<Dominus.ProjectFollower.ProjectFollower> ProjectFollowers { get; set; } = new List<Dominus.ProjectFollower.ProjectFollower>();
    public virtual ICollection<Dominus.ProjectCommunication.ProjectCommunication> ProjectCommunications { get; set; } = new List<Dominus.ProjectCommunication.ProjectCommunication>();

    protected Project()
    {
        // Required by EF Core
    }

    public Project(Guid id) : base(id)
    {
    }
}
