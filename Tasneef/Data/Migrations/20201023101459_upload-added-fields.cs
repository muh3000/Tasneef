using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasneef.Data.Migrations
{
    public partial class uploadaddedfields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_Messages_MessageId",
                table: "Uploads");

            migrationBuilder.AlterColumn<int>(
                name: "MessageId",
                table: "Uploads",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "Uploads",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Uploads",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_CustomerId",
                table: "Uploads",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Uploads_ProjectId",
                table: "Uploads",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_Customers_CustomerId",
                table: "Uploads",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_Messages_MessageId",
                table: "Uploads",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_Projects_ProjectId",
                table: "Uploads",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_Customers_CustomerId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_Messages_MessageId",
                table: "Uploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Uploads_Projects_ProjectId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_CustomerId",
                table: "Uploads");

            migrationBuilder.DropIndex(
                name: "IX_Uploads_ProjectId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Uploads");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Uploads");

            migrationBuilder.AlterColumn<int>(
                name: "MessageId",
                table: "Uploads",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Uploads_Messages_MessageId",
                table: "Uploads",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
