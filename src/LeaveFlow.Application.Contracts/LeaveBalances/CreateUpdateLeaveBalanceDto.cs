using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveFlow.LeaveBalances;

public class CreateUpdateLeaveBalanceDto
{
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public Guid LeaveTypeId { get; set; }

    [Required]
    [Range(2020, 2100)]
    public int Year { get; set; }

    [Required]
    [Range(0, 365)]
    public decimal TotalDays { get; set; }
}
