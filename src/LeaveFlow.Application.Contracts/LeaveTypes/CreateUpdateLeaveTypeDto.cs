using System.ComponentModel.DataAnnotations;

namespace LeaveFlow.LeaveTypes;

public class CreateUpdateLeaveTypeDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(1, 365)]
    public int DefaultDays { get; set; }

    public bool IsPaid { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
