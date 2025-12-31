namespace LeaveFlow;

public static class LeaveFlowDomainErrorCodes
{
    // LeaveRequest errors
    public const string StartDateMustBeBeforeEndDate = "LeaveFlow:00001";
    public const string CannotRequestLeaveInPast = "LeaveFlow:00002";
    public const string OnlyPendingRequestsCanBeApproved = "LeaveFlow:00003";
    public const string OnlyPendingRequestsCanBeRejected = "LeaveFlow:00004";
    public const string OnlyCancelPendingRequests = "LeaveFlow:00005";
    public const string InsufficientLeaveBalance = "LeaveFlow:00006";
    
    // LeaveType errors
    public const string LeaveTypeAlreadyExists = "LeaveFlow:00101";
    public const string LeaveTypeNotFound = "LeaveFlow:00102";
}
