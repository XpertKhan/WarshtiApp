using Microsoft.EntityFrameworkCore.Migrations;

namespace Musan.Entities.Migrations
{
    public partial class WorkflowAnnuouncementRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                schema: "Maintenance",
                table: "WorkShopInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvgRating",
                schema: "Maintenance",
                table: "WorkShopInfo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WorkshopId",
                table: "Announcements",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_WorkshopId",
                table: "Announcements",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_WorkShopInfo_WorkshopId",
                table: "Announcements",
                column: "WorkshopId",
                principalSchema: "Maintenance",
                principalTable: "WorkShopInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_WorkShopInfo_WorkshopId",
                table: "Announcements");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_WorkshopId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "Address",
                schema: "Maintenance",
                table: "WorkShopInfo");

            migrationBuilder.DropColumn(
                name: "AvgRating",
                schema: "Maintenance",
                table: "WorkShopInfo");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Announcements");
        }
    }
}
