using System;
using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveBalances;

public class GetLeaveBalanceListDto : PagedAndSortedResultRequestDto
{
    public Guid? UserId { get; set; }
    public Guid? LeaveTypeId { get; set; }
    public int? Year { get; set; }
}
