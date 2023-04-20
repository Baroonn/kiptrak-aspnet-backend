using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KipTrak.Migrations
{
    public partial class AddVerificationCodeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VCode",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VCode",
                table: "Profiles");
        }
    }
}
