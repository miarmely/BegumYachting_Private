using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BegumYatch.Repository.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConciergeServiceDemands_AspNetUsers_AppUserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.DropIndex(
                name: "IX_ConciergeServiceDemands_AppUserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ConciergeServiceDemands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ConciergeServiceDemands_UserId",
                table: "ConciergeServiceDemands",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConciergeServiceDemands_AspNetUsers_UserId",
                table: "ConciergeServiceDemands",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConciergeServiceDemands_AspNetUsers_UserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.DropIndex(
                name: "IX_ConciergeServiceDemands_UserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ConciergeServiceDemands");

            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "ConciergeServiceDemands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConciergeServiceDemands_AppUserId",
                table: "ConciergeServiceDemands",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConciergeServiceDemands_AspNetUsers_AppUserId",
                table: "ConciergeServiceDemands",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
