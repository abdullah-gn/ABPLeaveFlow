using System;
using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveRequests;

public class GetLeaveRequestListDto : PagedAndSortedResultRequestDto
{
    public Guid? LeaveTypeId { get; set; }
    public Guid? RequesterId { get; set; }
    public LeaveRequestStatus? Status { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
}
