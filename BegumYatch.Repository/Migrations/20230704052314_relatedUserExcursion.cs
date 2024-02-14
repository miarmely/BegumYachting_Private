using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BegumYatch.Repository.Migrations
{
    public partial class relatedUserExcursion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ExcursionDemands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExcursionDemands_UserId",
                table: "ExcursionDemands",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExcursionDemands_AspNetUsers_UserId",
                table: "ExcursionDemands",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExcursionDemands_AspNetUsers_UserId",
                table: "ExcursionDemands");

            migrationBuilder.DropIndex(
                name: "IX_ExcursionDemands_UserId",
                table: "ExcursionDemands");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExcursionDemands");
        }
    }
}
