using Volo.Abp.Application.Dtos;

namespace LeaveFlow.LeaveTypes;

public class GetLeaveTypeListDto : PagedAndSortedResultRequestDto
{
    public string? Filter { get; set; }
    public bool? IsActive { get; set; }
}
