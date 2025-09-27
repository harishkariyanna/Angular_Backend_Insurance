using Microsoft.EntityFrameworkCore.Migrations;
namespace insu.Migrations
{
    public partial class AddDocumentFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AadharCardPath",
                table: "PolicyApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BankPassbookPath",
                table: "PolicyApplications",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PanCardPath",
                table: "PolicyApplications",
                type: "nvarchar(max)",
                nullable: true);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AadharCardPath",
                table: "PolicyApplications");

            migrationBuilder.DropColumn(
                name: "BankPassbookPath",
                table: "PolicyApplications");

            migrationBuilder.DropColumn(
                name: "PanCardPath",
                table: "PolicyApplications");
        }
    }
}