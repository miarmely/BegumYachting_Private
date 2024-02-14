using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BegumYatch.Repository.Migrations
{
    public partial class RelationUserFuelPurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FuelPurchaseDemands",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FuelPurchaseDemands_UserId",
                table: "FuelPurchaseDemands",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FuelPurchaseDemands_AspNetUsers_UserId",
                table: "FuelPurchaseDemands",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FuelPurchaseDemands_AspNetUsers_UserId",
                table: "FuelPurchaseDemands");

            migrationBuilder.DropIndex(
                name: "IX_FuelPurchaseDemands_UserId",
                table: "FuelPurchaseDemands");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FuelPurchaseDemands");
        }
    }
}
