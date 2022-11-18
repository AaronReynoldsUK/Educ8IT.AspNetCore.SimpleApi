using Microsoft.EntityFrameworkCore.Migrations;

namespace Educ8IT.AspNetCore.SimpleApi.Identity.Migrations
{
    public partial class UpdateApiMfa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "ApiMfas",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicInfo",
                table: "ApiMfas",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "ApiMfas");

            migrationBuilder.DropColumn(
                name: "PublicInfo",
                table: "ApiMfas");
        }
    }
}
