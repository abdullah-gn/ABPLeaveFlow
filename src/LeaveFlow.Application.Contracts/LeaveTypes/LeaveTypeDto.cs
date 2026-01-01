using System;
using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveTypes;

public class LeaveTypeDto : FullAuditedEntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int DefaultDays { get; set; }
    public bool IsPaid { get; set; }
    public bool IsActive { get; set; }
}
