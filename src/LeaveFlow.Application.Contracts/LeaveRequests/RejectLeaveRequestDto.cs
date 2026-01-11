using System.ComponentModel.DataAnnotations;

namespace LeaveFlow.LeaveRequests;

public class RejectLeaveRequestDto
{
    [Required]
    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}
