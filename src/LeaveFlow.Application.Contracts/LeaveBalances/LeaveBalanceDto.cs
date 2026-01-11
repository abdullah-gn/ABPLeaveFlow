using System;
using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveBalances;

public class LeaveBalanceDto : FullAuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public Guid LeaveTypeId { get; set; }
    public string LeaveTypeName { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal TotalDays { get; set; }
    public decimal UsedDays { get; set; }
    public decimal RemainingDays { get; set; }
}
