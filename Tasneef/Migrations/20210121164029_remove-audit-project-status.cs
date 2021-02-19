using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Tasneef.Migrations
{
    public partial class removeauditprojectstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatuses_AspNetUsers_CreatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectStatuses_AspNetUsers_UpdatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ProjectStatuses_CreatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropIndex(
                name: "IX_ProjectStatuses_UpdatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "ProjectStatuses");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ProjectStatuses");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_CustomerId",
                table: "Subscriptions",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_Customers_CustomerId",
                table: "Subscriptions",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_Customers_CustomerId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_CustomerId",
                table: "Subscriptions");

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ProjectStatuses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ProjectStatuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "ProjectStatuses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ProjectStatuses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatuses_CreatedById",
                table: "ProjectStatuses",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatuses_UpdatedById",
                table: "ProjectStatuses",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatuses_AspNetUsers_CreatedById",
                table: "ProjectStatuses",
                column: "CreatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectStatuses_AspNetUsers_UpdatedById",
                table: "ProjectStatuses",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
