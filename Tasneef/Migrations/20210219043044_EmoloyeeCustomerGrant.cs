using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasneef.Migrations
{
    public partial class EmoloyeeCustomerGrant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeCustomerGrants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeCustomerGrants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeCustomerGrants_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeCustomerGrants_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCustomerGrants_CustomerId",
                table: "EmployeeCustomerGrants",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCustomerGrants_EmployeeId",
                table: "EmployeeCustomerGrants",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmployeeCustomerGrants");
        }
    }
}
