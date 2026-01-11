using System.ComponentModel.DataAnnotations;

namespace LeaveFlow.LeaveRequests;

public class ApproveLeaveRequestDto
{
    [StringLength(500)]
    public string? Notes { get; set; }
}
