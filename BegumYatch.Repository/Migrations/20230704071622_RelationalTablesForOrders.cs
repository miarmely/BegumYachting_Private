using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BegumYatch.Repository.Migrations
{
    public partial class RelationalTablesForOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TechnicalAssitanceandSparePartOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "ProvisionOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "FlowerOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TechnicalAssitanceandSparePartOrders_UserId",
                table: "TechnicalAssitanceandSparePartOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProvisionOrders_UserId",
                table: "ProvisionOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowerOrders_UserId",
                table: "FlowerOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlowerOrders_AspNetUsers_UserId",
                table: "FlowerOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProvisionOrders_AspNetUsers_UserId",
                table: "ProvisionOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalAssitanceandSparePartOrders_AspNetUsers_UserId",
                table: "TechnicalAssitanceandSparePartOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlowerOrders_AspNetUsers_UserId",
                table: "FlowerOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProvisionOrders_AspNetUsers_UserId",
                table: "ProvisionOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalAssitanceandSparePartOrders_AspNetUsers_UserId",
                table: "TechnicalAssitanceandSparePartOrders");

            migrationBuilder.DropIndex(
                name: "IX_TechnicalAssitanceandSparePartOrders_UserId",
                table: "TechnicalAssitanceandSparePartOrders");

            migrationBuilder.DropIndex(
                name: "IX_ProvisionOrders_UserId",
                table: "ProvisionOrders");

            migrationBuilder.DropIndex(
                name: "IX_FlowerOrders_UserId",
                table: "FlowerOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TechnicalAssitanceandSparePartOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProvisionOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FlowerOrders");
        }
    }
}
