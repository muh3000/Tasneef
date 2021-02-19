using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasneef.Migrations
{
    public partial class emailConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailConfigurations",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FromEmail = table.Column<string>(nullable: true),
                    SenderName = table.Column<string>(nullable: true),
                    SMTPUername = table.Column<string>(nullable: true),
                    SMTPPassword = table.Column<string>(nullable: true),
                    SMTPServer = table.Column<string>(nullable: true),
                    SMTPPort = table.Column<int>(nullable: false),
                    EnableSSL = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfigurations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailConfigurations");
        }
    }
}
