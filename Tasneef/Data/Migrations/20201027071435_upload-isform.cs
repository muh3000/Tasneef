using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasneef.Data.Migrations
{
    public partial class uploadisform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsForm",
                table: "Uploads",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsForm",
                table: "Uploads");
        }
    }
}
