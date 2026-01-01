using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace LeaveFlow.LeaveTypes;

public class LeaveTypeDataSeederContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<LeaveType, Guid> _leaveTypeRepository;
    private readonly IGuidGenerator _guidGenerator;

    public LeaveTypeDataSeederContributor(
        IRepository<LeaveType, Guid> leaveTypeRepository,
        IGuidGenerator guidGenerator)
    {
        _leaveTypeRepository = leaveTypeRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _leaveTypeRepository.GetCountAsync() > 0)
        {
            return;
        }

        // Annual Leave
        await _leaveTypeRepository.InsertAsync(
            new LeaveType(
                _guidGenerator.Create(),
                "Annual Leave",
                defaultDays: 21,
                isPaid: true
            )
            {
                Description = "Yearly vacation leave for employees"
            },
            autoSave: true
        );

        // Sick Leave
        await _leaveTypeRepository.InsertAsync(
            new LeaveType(
                _guidGenerator.Create(),
                "Sick Leave",
                defaultDays: 14,
                isPaid: true
            )
            {
                Description = "Leave for medical reasons and illness"
            },
            autoSave: true
        );

        // Unpaid Leave
        await _leaveTypeRepository.InsertAsync(
            new LeaveType(
                _guidGenerator.Create(),
                "Unpaid Leave",
                defaultDays: 30,
                isPaid: false
            )
            {
                Description = "Leave without pay for personal reasons"
            },
            autoSave: true
        );

        // Maternity Leave
        await _leaveTypeRepository.InsertAsync(
            new LeaveType(
                _guidGenerator.Create(),
                "Maternity Leave",
                defaultDays: 90,
                isPaid: true
            )
            {
                Description = "Leave for expecting mothers"
            },
            autoSave: true
        );

        // Paternity Leave
        await _leaveTypeRepository.InsertAsync(
            new LeaveType(
                _guidGenerator.Create(),
                "Paternity Leave",
                defaultDays: 5,
                isPaid: true
            )
            {
                Description = "Leave for new fathers"
            },
            autoSave: true
        );
    }
}
