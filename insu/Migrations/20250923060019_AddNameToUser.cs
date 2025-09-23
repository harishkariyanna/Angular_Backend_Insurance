using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace insu.Migrations
{
    /// <inheritdoc />
    public partial class AddNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User");
                
            // Update existing users with default names
            migrationBuilder.Sql("UPDATE Users SET Name = 'Admin User' WHERE Email = 'admin@insure.com'");
            migrationBuilder.Sql("UPDATE Users SET Name = 'Agent User' WHERE Email = 'agent@insure.com'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Policies",
                columns: new[] { "Id", "CreatedAt", "Description", "ImagePath", "Name", "Premium", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3670), "Basic health coverage", null, "Health Basic Plan", 299.99m, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3670) },
                    { 2, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3677), "Comprehensive auto insurance", null, "Auto Shield", 599.99m, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3677) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3323), "admin@insure.com", "Admin@123", "Admin", new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3325) },
                    { 2, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3330), "agent@insure.com", "Agent@123", "Agent", new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3330) }
                });

            migrationBuilder.InsertData(
                table: "PolicyApplications",
                columns: new[] { "Id", "AdminComments", "AgentId", "CreatedAt", "DurationMonths", "PolicyId", "Status", "UpdatedAt", "UserId" },
                values: new object[] { 1, null, null, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3715), 12, 1, "Approved", new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3716), 1 });

            migrationBuilder.InsertData(
                table: "Claims",
                columns: new[] { "Id", "Amount", "CreatedAt", "Description", "PolicyApplicationId", "PolicyId", "Status", "Title", "UpdatedAt", "UserId", "UserId1" },
                values: new object[] { 1, 1000m, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3757), "Sample claim for testing", 1, null, "Pending", "Test Claim", new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3758), 1, null });
        }
    }
}
