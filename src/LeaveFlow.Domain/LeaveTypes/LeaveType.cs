using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeaveFlow.LeaveTypes;

public class LeaveType : FullAuditedAggregateRoot<Guid>
{
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; set; }
    public int DefaultDays { get; set; }
    public bool IsPaid { get; set; }
    public bool IsActive { get; set; }

    // Private constructor for EF Core
    private LeaveType() { }

    public LeaveType(Guid id, string name, int defaultDays, bool isPaid = true)
        : base(id)
    {
        SetName(name);
        DefaultDays = defaultDays;
        IsPaid = isPaid;
        IsActive = true;
    }

    public void SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), maxLength: 100);
    }
}
