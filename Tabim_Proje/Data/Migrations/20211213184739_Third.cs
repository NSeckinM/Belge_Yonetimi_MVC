using Microsoft.EntityFrameworkCore.Migrations;

namespace Tabim_Proje.Data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResultOfConsideration",
                table: "UserRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultOfConsideration",
                table: "UserRequests");
        }
    }
}
