using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveFlow.LeaveRequests;

public class CreateLeaveRequestDto
{
    [Required]
    public Guid LeaveTypeId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [StringLength(500)]
    public string Reason { get; set; } = string.Empty;
}
