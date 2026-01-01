namespace LeaveFlow.Permissions;

public static class LeaveFlowPermissions
{
    public const string GroupName = "LeaveFlow";

    public static class LeaveTypes
    {
        public const string Default = GroupName + ".LeaveTypes";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class LeaveRequests
    {
        public const string Default = GroupName + ".LeaveRequests";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string Approve = Default + ".Approve";
    }

    public static class LeaveBalances
    {
        public const string Default = GroupName + ".LeaveBalances";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
    }
}
