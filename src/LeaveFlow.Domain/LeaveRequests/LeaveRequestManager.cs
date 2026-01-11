using System;
using System.Threading.Tasks;
using LeaveFlow.LeaveBalances;
using LeaveFlow.LeaveTypes;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace LeaveFlow.LeaveRequests;

public class LeaveRequestManager : DomainService
{
    private readonly ILeaveRequestRepository _leaveRequestRepository;
    private readonly ILeaveTypeRepository _leaveTypeRepository;
    private readonly ILeaveBalanceRepository _leaveBalanceRepository;

    public LeaveRequestManager(
        ILeaveRequestRepository leaveRequestRepository,
        ILeaveTypeRepository leaveTypeRepository,
        ILeaveBalanceRepository leaveBalanceRepository)
    {
        _leaveRequestRepository = leaveRequestRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _leaveBalanceRepository = leaveBalanceRepository;
    }

    public async Task<LeaveRequest> CreateAsync(
        Guid leaveTypeId,
        Guid requesterId,
        DateTime startDate,
        DateTime endDate,
        string reason)
    {
        // Validate leave type exists and is active
        var leaveType = await _leaveTypeRepository.GetAsync(leaveTypeId);
        if (!leaveType.IsActive)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.LeaveTypeNotFound)
                .WithData("LeaveTypeId", leaveTypeId);
        }

        // Check for overlapping requests
        var overlappingRequests = await _leaveRequestRepository.GetOverlappingRequestsAsync(
            requesterId, startDate, endDate);
        
        if (overlappingRequests.Count > 0)
        {
            throw new BusinessException("LeaveFlow:00007")
                .WithData("StartDate", startDate)
                .WithData("EndDate", endDate);
        }

        // Calculate total days
        var totalDays = (endDate - startDate).Days + 1;

        // Check leave balance (for the year of start date)
        var year = startDate.Year;
        var balance = await _leaveBalanceRepository.GetByUserAndLeaveTypeAsync(
            requesterId, leaveTypeId, year);

        if (balance == null)
        {
            throw new BusinessException("LeaveFlow:00008")
                .WithData("Year", year)
                .WithData("LeaveType", leaveType.Name);
        }

        if (balance.RemainingDays < totalDays)
        {
            throw new BusinessException(LeaveFlowDomainErrorCodes.InsufficientLeaveBalance)
                .WithData("Required", totalDays)
                .WithData("Available", balance.RemainingDays);
        }

        // Create the leave request
        var leaveRequest = new LeaveRequest(
            GuidGenerator.Create(),
            leaveTypeId,
            requesterId,
            startDate,
            endDate,
            reason
        );

        return leaveRequest;
    }

    public async Task ApproveAsync(LeaveRequest leaveRequest, Guid approverId, string? notes)
    {
        // Approve the request (this validates state)
        leaveRequest.Approve(approverId, notes);

        // Deduct from balance
        var year = leaveRequest.StartDate.Year;
        var balance = await _leaveBalanceRepository.GetByUserAndLeaveTypeAsync(
            leaveRequest.RequesterId, leaveRequest.LeaveTypeId, year);

        if (balance != null)
        {
            balance.DeductDays(leaveRequest.TotalDays);
            await _leaveBalanceRepository.UpdateAsync(balance);
        }
    }

    public async Task RejectAsync(LeaveRequest leaveRequest, Guid approverId, string notes)
    {
        leaveRequest.Reject(approverId, notes);
    }

    public async Task CancelAsync(LeaveRequest leaveRequest)
    {
        var wasApproved = leaveRequest.Status == LeaveRequestStatus.Approved;
        
        leaveRequest.Cancel();

        // Restore balance if it was approved
        if (wasApproved)
        {
            var year = leaveRequest.StartDate.Year;
            var balance = await _leaveBalanceRepository.GetByUserAndLeaveTypeAsync(
                leaveRequest.RequesterId, leaveRequest.LeaveTypeId, year);

            if (balance != null)
            {
                balance.RestoreDays(leaveRequest.TotalDays);
                await _leaveBalanceRepository.UpdateAsync(balance);
            }
        }
    }
}
