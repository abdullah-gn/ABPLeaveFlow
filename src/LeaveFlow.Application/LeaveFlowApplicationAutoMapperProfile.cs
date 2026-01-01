using AutoMapper;
using LeaveFlow.LeaveTypes;

namespace LeaveFlow;

public class LeaveFlowApplicationAutoMapperProfile : Profile
{
    public LeaveFlowApplicationAutoMapperProfile()
    {
        // LeaveType mappings
        CreateMap<LeaveType, LeaveTypeDto>();
        CreateMap<CreateUpdateLeaveTypeDto, LeaveType>();
    }
}
