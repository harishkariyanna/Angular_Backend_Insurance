using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace insu.Migrations
{
    /// <inheritdoc />
    public partial class Second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PolicyApplications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    DurationMonths = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdminComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PolicyApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PolicyApplications_Policies_PolicyId",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PolicyApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9780), new DateTime(2025, 9, 22, 7, 13, 55, 417, DateTimeKind.Utc).AddTicks(9780) });

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

            migrationBuilder.CreateIndex(
                name: "IX_PolicyApplications_PolicyId",
                table: "PolicyApplications",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PolicyApplications_UserId",
                table: "PolicyApplications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PolicyApplications");

            migrationBuilder.UpdateData(
                table: "Claims",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6521), new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6522) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6478), new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6479) });

            migrationBuilder.UpdateData(
                table: "Policies",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6484), new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6485) });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6232), new DateTime(2025, 9, 22, 6, 16, 54, 879, DateTimeKind.Utc).AddTicks(6234) });
        }
    }
}
