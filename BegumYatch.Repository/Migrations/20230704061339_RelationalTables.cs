using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BegumYatch.Repository.Migrations
{
    public partial class RelationalTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "SecurityServiceDemands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SecurityServiceDemands_UserId",
                table: "SecurityServiceDemands",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecurityServiceDemands_AspNetUsers_UserId",
                table: "SecurityServiceDemands",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SecurityServiceDemands_AspNetUsers_UserId",
                table: "SecurityServiceDemands");

            migrationBuilder.DropIndex(
                name: "IX_SecurityServiceDemands_UserId",
                table: "SecurityServiceDemands");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SecurityServiceDemands");
        }
    }
}
