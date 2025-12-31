using System;
using LeaveFlow.LeaveTypes;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeaveFlow.LeaveBalances;

public class LeaveBalance : FullAuditedEntity<Guid>
{
    public Guid UserId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public int Year { get; private set; }
    public decimal TotalDays { get; private set; }
    public decimal UsedDays { get; private set; }
    public decimal RemainingDays => TotalDays - UsedDays;

    // Navigation property
    public virtual LeaveType LeaveType { get; private set; } = null!;

    // Private constructor for EF Core
    private LeaveBalance() { }

    public LeaveBalance(Guid id, Guid userId, Guid leaveTypeId, int year, decimal totalDays)
        : base(id)
    {
        UserId = userId;
        LeaveTypeId = leaveTypeId;
        Year = year;
        TotalDays = totalDays;
        UsedDays = 0;
    }

    public void DeductDays(decimal days)
    {
        if (days > RemainingDays)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.InsufficientLeaveBalance);
        }

        UsedDays += days;
    }

    public void RestoreDays(decimal days)
    {
        UsedDays -= days;
        if (UsedDays < 0)
        {
            UsedDays = 0;
        }
    }

    public void AdjustTotalDays(decimal newTotal)
    {
        TotalDays = newTotal;
    }
}
