using System;
using LeaveFlow.LeaveTypes;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace LeaveFlow.LeaveRequests;

public class LeaveRequest : FullAuditedAggregateRoot<Guid>
{
    public Guid LeaveTypeId { get; private set; }
    public Guid RequesterId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveRequestStatus Status { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public string? ApproverNotes { get; private set; }
    public Guid? ApproverId { get; private set; }
    public DateTime? DecisionDate { get; private set; }

    // Calculated property
    public int TotalDays => (EndDate - StartDate).Days + 1;

    // Navigation property
    public virtual LeaveType LeaveType { get; private set; } = null!;

    // Private constructor for EF Core
    private LeaveRequest() { }

    public LeaveRequest(
        Guid id,
        Guid leaveTypeId,
        Guid requesterId,
        DateTime startDate,
        DateTime endDate,
        string reason)
        : base(id)
    {
        LeaveTypeId = leaveTypeId;
        RequesterId = requesterId;
        SetDates(startDate, endDate);
        Reason = Check.NotNullOrWhiteSpace(reason, nameof(reason), maxLength: 500);
        Status = LeaveRequestStatus.Pending;
    }

    public void SetDates(DateTime startDate, DateTime endDate)
    {
        if (startDate > endDate)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.StartDateMustBeBeforeEndDate);
        }

        if (startDate.Date < DateTime.Today)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.CannotRequestLeaveInPast);
        }

        StartDate = startDate;
        EndDate = endDate;
    }

    public void Approve(Guid approverId, string? notes = null)
    {
        if (Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.OnlyPendingRequestsCanBeApproved);
        }

        Status = LeaveRequestStatus.Approved;
        ApproverId = approverId;
        ApproverNotes = notes;
        DecisionDate = DateTime.Now;
    }

    public void Reject(Guid approverId, string notes)
    {
        if (Status != LeaveRequestStatus.Pending)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.OnlyPendingRequestsCanBeRejected);
        }

        Status = LeaveRequestStatus.Rejected;
        ApproverId = approverId;
        ApproverNotes = Check.NotNullOrWhiteSpace(notes, nameof(notes), maxLength: 500);
        DecisionDate = DateTime.Now;
    }

    public void Cancel()
    {
        // Allow canceling both pending and approved (if leave hasn't started yet)
        if (Status == LeaveRequestStatus.Rejected || Status == LeaveRequestStatus.Cancelled)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.OnlyCancelPendingRequests);
        }

        // Cannot cancel if leave has already started
        if (Status == LeaveRequestStatus.Approved && StartDate.Date <= DateTime.Today)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.CannotCancelApprovedLeave);
        }

        Status = LeaveRequestStatus.Cancelled;
        DecisionDate = DateTime.Now;
    }

    public bool WasApproved => Status == LeaveRequestStatus.Approved || 
                               (Status == LeaveRequestStatus.Cancelled && ApproverId.HasValue);
}
