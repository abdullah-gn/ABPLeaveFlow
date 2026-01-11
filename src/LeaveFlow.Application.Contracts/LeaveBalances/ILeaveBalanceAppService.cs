using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace LeaveFlow.LeaveBalances;

public interface ILeaveBalanceAppService : IApplicationService
{
    Task<LeaveBalanceDto> GetAsync(Guid id);
    
    Task<PagedResultDto<LeaveBalanceDto>> GetListAsync(GetLeaveBalanceListDto input);
    
    Task<LeaveBalanceDto> CreateAsync(CreateUpdateLeaveBalanceDto input);
    
    Task<LeaveBalanceDto> UpdateAsync(Guid id, CreateUpdateLeaveBalanceDto input);
    
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Get all balances for the current logged-in user for the current year
    /// </summary>
    Task<List<LeaveBalanceDto>> GetMyBalancesAsync(int? year = null);
    
    /// <summary>
    /// Initialize balances for a user based on all active leave types
    /// </summary>
    Task InitializeBalancesForUserAsync(Guid userId, int year);
}
