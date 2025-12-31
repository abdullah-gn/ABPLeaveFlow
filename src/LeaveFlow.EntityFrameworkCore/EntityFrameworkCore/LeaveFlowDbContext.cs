using LeaveFlow.LeaveBalances;
using LeaveFlow.LeaveRequests;
using LeaveFlow.LeaveTypes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace LeaveFlow.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class LeaveFlowDbContext :
    AbpDbContext<LeaveFlowDbContext>,
    IIdentityDbContext,
    ITenantManagementDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */
    
    // LeaveFlow Entities
    public DbSet<LeaveType> LeaveTypes { get; set; }
    public DbSet<LeaveRequest> LeaveRequests { get; set; }
    public DbSet<LeaveBalance> LeaveBalances { get; set; }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }
    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public LeaveFlowDbContext(DbContextOptions<LeaveFlowDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();
        builder.ConfigureTenantManagement();

        /* Configure your own tables/entities inside here */

        // LeaveType Configuration
        builder.Entity<LeaveType>(b =>
        {
            b.ToTable(LeaveFlowConsts.DbTablePrefix + "LeaveTypes", LeaveFlowConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            b.Property(x => x.Description)
                .HasMaxLength(500);
            
            b.HasIndex(x => x.Name);
        });

        // LeaveRequest Configuration
        builder.Entity<LeaveRequest>(b =>
        {
            b.ToTable(LeaveFlowConsts.DbTablePrefix + "LeaveRequests", LeaveFlowConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.Reason)
                .IsRequired()
                .HasMaxLength(500);
            
            b.Property(x => x.ApproverNotes)
                .HasMaxLength(500);
            
            b.Property(x => x.Status)
                .IsRequired();
            
            // Foreign key relationship
            b.HasOne(x => x.LeaveType)
                .WithMany()
                .HasForeignKey(x => x.LeaveTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            b.HasIndex(x => x.RequesterId);
            b.HasIndex(x => x.Status);
            b.HasIndex(x => new { x.StartDate, x.EndDate });
        });

        // LeaveBalance Configuration
        builder.Entity<LeaveBalance>(b =>
        {
            b.ToTable(LeaveFlowConsts.DbTablePrefix + "LeaveBalances", LeaveFlowConsts.DbSchema);
            b.ConfigureByConvention();
            
            b.Property(x => x.TotalDays)
                .HasPrecision(5, 2);
            
            b.Property(x => x.UsedDays)
                .HasPrecision(5, 2);
            
            // Foreign key relationship
            b.HasOne(x => x.LeaveType)
                .WithMany()
                .HasForeignKey(x => x.LeaveTypeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            
            // Unique constraint: one balance per user per leave type per year
            b.HasIndex(x => new { x.UserId, x.LeaveTypeId, x.Year })
                .IsUnique();
        });
    }
}
