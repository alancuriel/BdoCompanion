using Microsoft.EntityFrameworkCore.Migrations;

namespace uwpUI.Core.Migrations
{
    public partial class item_know_sell_buy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BuyPrice",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Knowledge",
                table: "Items",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellPrice",
                table: "Items",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "Knowledge",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SellPrice",
                table: "Items");
        }
    }
}
