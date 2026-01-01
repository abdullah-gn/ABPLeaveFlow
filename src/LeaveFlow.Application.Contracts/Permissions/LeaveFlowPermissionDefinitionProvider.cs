using LeaveFlow.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace LeaveFlow.Permissions;

public class LeaveFlowPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var leaveFlowGroup = context.AddGroup(LeaveFlowPermissions.GroupName, L("Permission:LeaveFlow"));

        // LeaveTypes permissions
        var leaveTypesPermission = leaveFlowGroup.AddPermission(
            LeaveFlowPermissions.LeaveTypes.Default, L("Permission:LeaveTypes"));
        leaveTypesPermission.AddChild(
            LeaveFlowPermissions.LeaveTypes.Create, L("Permission:LeaveTypes.Create"));
        leaveTypesPermission.AddChild(
            LeaveFlowPermissions.LeaveTypes.Edit, L("Permission:LeaveTypes.Edit"));
        leaveTypesPermission.AddChild(
            LeaveFlowPermissions.LeaveTypes.Delete, L("Permission:LeaveTypes.Delete"));

        // LeaveRequests permissions
        var leaveRequestsPermission = leaveFlowGroup.AddPermission(
            LeaveFlowPermissions.LeaveRequests.Default, L("Permission:LeaveRequests"));
        leaveRequestsPermission.AddChild(
            LeaveFlowPermissions.LeaveRequests.Create, L("Permission:LeaveRequests.Create"));
        leaveRequestsPermission.AddChild(
            LeaveFlowPermissions.LeaveRequests.Edit, L("Permission:LeaveRequests.Edit"));
        leaveRequestsPermission.AddChild(
            LeaveFlowPermissions.LeaveRequests.Delete, L("Permission:LeaveRequests.Delete"));
        leaveRequestsPermission.AddChild(
            LeaveFlowPermissions.LeaveRequests.Approve, L("Permission:LeaveRequests.Approve"));

        // LeaveBalances permissions
        var leaveBalancesPermission = leaveFlowGroup.AddPermission(
            LeaveFlowPermissions.LeaveBalances.Default, L("Permission:LeaveBalances"));
        leaveBalancesPermission.AddChild(
            LeaveFlowPermissions.LeaveBalances.Create, L("Permission:LeaveBalances.Create"));
        leaveBalancesPermission.AddChild(
            LeaveFlowPermissions.LeaveBalances.Edit, L("Permission:LeaveBalances.Edit"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<LeaveFlowResource>(name);
    }
}
