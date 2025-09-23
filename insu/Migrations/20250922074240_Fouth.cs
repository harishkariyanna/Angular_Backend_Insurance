using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace insu.Migrations
{
    /// <inheritdoc />
    public partial class Fouth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Policies_PolicyId",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "Claims",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PolicyApplicationId",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PolicyApplicationId", "PolicyId", "UpdatedAt", "UserId1" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(185), 1, null, new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(185), null });

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

            migrationBuilder.InsertData(
                table: "PolicyApplications",
                columns: new[] { "Id", "AdminComments", "CreatedAt", "DurationMonths", "PolicyId", "Status", "UpdatedAt", "UserId" },
                values: new object[] { 1, null, new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(167), 12, 1, "Approved", new DateTime(2025, 9, 22, 7, 42, 40, 84, DateTimeKind.Utc).AddTicks(167), 1 });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 42, 40, 83, DateTimeKind.Utc).AddTicks(9873), new DateTime(2025, 9, 22, 7, 42, 40, 83, DateTimeKind.Utc).AddTicks(9876) });

            migrationBuilder.CreateIndex(
                name: "IX_Claims_PolicyApplicationId",
                table: "Claims",
                column: "PolicyApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Claims_UserId1",
                table: "Claims",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Policies_PolicyId",
                table: "Claims",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_PolicyApplications_PolicyApplicationId",
                table: "Claims",
                column: "PolicyApplicationId",
                principalTable: "PolicyApplications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Users_UserId1",
                table: "Claims",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Policies_PolicyId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_PolicyApplications_PolicyApplicationId",
                table: "Claims");

            migrationBuilder.DropForeignKey(
                name: "FK_Claims_Users_UserId1",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_PolicyApplicationId",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_UserId1",
                table: "Claims");

            migrationBuilder.DeleteData(
                table: "PolicyApplications",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "PolicyApplicationId",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Claims");

            migrationBuilder.AlterColumn<int>(
                name: "PolicyId",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PolicyId", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9780), 1, new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9780) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9755), new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9755) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9758), new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9758) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9589), new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9593) });

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_Policies_PolicyId",
                table: "Claims",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
