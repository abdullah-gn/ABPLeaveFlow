using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveFlow.Migrations
{
    /// <inheritdoc />
    public partial class Added_LeaveFlow_Entities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppLeaveTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DefaultDays = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLeaveBalances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    TotalDays = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    UsedDays = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLeaveBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLeaveBalances_AppLeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "AppLeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppLeaveRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeaveTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequesterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ApproverNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ApproverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DecisionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLeaveRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLeaveRequests_AppLeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "AppLeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveBalances_LeaveTypeId",
                table: "AppLeaveBalances",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveBalances_UserId_LeaveTypeId_Year",
                table: "AppLeaveBalances",
                columns: new[] { "UserId", "LeaveTypeId", "Year" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveRequests_LeaveTypeId",
                table: "AppLeaveRequests",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveRequests_RequesterId",
                table: "AppLeaveRequests",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveRequests_StartDate_EndDate",
                table: "AppLeaveRequests",
                columns: new[] { "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveRequests_Status",
                table: "AppLeaveRequests",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AppLeaveTypes_Name",
                table: "AppLeaveTypes",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLeaveBalances");

            migrationBuilder.DropTable(
                name: "AppLeaveRequests");

            migrationBuilder.DropTable(
                name: "AppLeaveTypes");
        }
    }
}
