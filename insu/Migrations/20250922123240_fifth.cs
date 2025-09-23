using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace insu.Migrations
{
    /// <inheritdoc />
    public partial class fifth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgentId",
                table: "PolicyApplications",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3757), new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3758) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3670), new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3670) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3677), new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3677) });

            migrationBuilder.UpdateData(
                table: "PolicyApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AgentId", "CreatedAt", "UpdatedAt" },
                values: new object[] { null, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3715), new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3716) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3323), new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3325) });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Password", "Role", "UpdatedAt" },
                values: new object[] { 2, new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3330), "agent@insure.com", "Agent@123", "Agent", new DateTime(2025, 9, 22, 12, 32, 39, 569, DateTimeKind.Utc).AddTicks(3330) });

            migrationBuilder.CreateIndex(
                name: "IX_PolicyApplications_AgentId",
                table: "PolicyApplications",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyApplications_Users_AgentId",
                table: "PolicyApplications",
                column: "AgentId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PolicyApplications_Users_AgentId",
                table: "PolicyApplications");

            migrationBuilder.DropIndex(
                name: "IX_PolicyApplications_AgentId",
                table: "PolicyApplications");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "AgentId",
                table: "PolicyApplications");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(185), new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(185) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(143), new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(144) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(147), new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(147) });

            migrationBuilder.UpdateData(
                table: "PolicyApplications",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(167), new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(167) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 83, DateTimeKind.Utc).AddTicks(9873), new DateTime(2025, 9, 22, 7, 42, 40, 83, DateTimeKind.Utc).AddTicks(9876) });
        }
    }
}
