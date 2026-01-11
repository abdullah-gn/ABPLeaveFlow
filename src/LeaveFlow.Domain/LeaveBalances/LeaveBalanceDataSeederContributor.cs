using System;
using System.Linq;
using System.Threading.Tasks;
using LeaveFlow.LeaveTypes;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace LeaveFlow.LeaveBalances;

public class LeaveBalanceDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<LeaveBalance, Guid> _leaveBalanceRepository;
    private readonly IRepository<LeaveType, Guid> _leaveTypeRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IGuidGenerator _guidGenerator;

    public LeaveBalanceDataSeederContributor(
        IRepository<LeaveBalance, Guid> leaveBalanceRepository,
        IRepository<LeaveType, Guid> leaveTypeRepository,
        IIdentityUserRepository userRepository,
        IGuidGenerator guidGenerator)
    {
        _leaveBalanceRepository = leaveBalanceRepository;
        _leaveTypeRepository = leaveTypeRepository;
        _userRepository = userRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        // Get admin user
        var adminUser = await _userRepository.FindByNormalizedUserNameAsync("ADMIN");
        if (adminUser == null)
        {
            return;
        }

        var currentYear = DateTime.Now.Year;

        // Check if admin already has balances for current year
        var existingBalances = await _leaveBalanceRepository.GetListAsync();
        if (existingBalances.Any(b => b.UserId == adminUser.Id && b.Year == currentYear))
        {
            return;
        }

        // Get all active leave types
        var leaveTypes = await _leaveTypeRepository.GetListAsync();
        var activeLeaveTypes = leaveTypes.Where(lt => lt.IsActive).ToList();

        foreach (var leaveType in activeLeaveTypes)
        {
            await _leaveBalanceRepository.InsertAsync(
                new LeaveBalance(
                    _guidGenerator.Create(),
                    adminUser.Id,
                    leaveType.Id,
                    currentYear,
                    leaveType.DefaultDays
                ),
                autoSave: true
            );
        }
    }
}
