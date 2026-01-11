using System;
using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveRequests;

public class LeaveRequestDto : FullAuditedEntityDto<Guid>
{
    public Guid LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
    public string RequesterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalDays { get; set; }
    public LeaveRequestStatus Status { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? ApproverNotes { get; set; }
    public Guid? ApproverId { get; set; }
    public string? ApproverName { get; set; }
    public DateTime? DecisionDate { get; set; }
}
