using AutoMapper;
using LeaveFlow.LeaveBalances;
using LeaveFlow.LeaveRequests;
using LeaveFlow.LeaveTypes;

namespace LeaveFlow;

public class LeaveFlowApplicationAutoMapperProfile : Profile
{
    public LeaveFlowApplicationAutoMapperProfile()
    {
        // LeaveType mappings
        CreateMap<LeaveType, LeaveTypeDto>();
        CreateMap<CreateUpdateLeaveTypeDto, LeaveType>();

        // LeaveRequest mappings
        CreateMap<LeaveRequest, LeaveRequestDto>()
            .ForMember(dest => dest.LeaveTypeName, opt => opt.Ignore())
            .ForMember(dest => dest.RequesterName, opt => opt.Ignore())
            .ForMember(dest => dest.ApproverName, opt => opt.Ignore());

        // LeaveBalance mappings
        CreateMap<LeaveBalance, LeaveBalanceDto>()
            .ForMember(dest => dest.UserName, opt => opt.Ignore())
            .ForMember(dest => dest.LeaveTypeName, opt => opt.Ignore());
    }
}
