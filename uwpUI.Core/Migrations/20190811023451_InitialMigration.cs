using Microsoft.EntityFrameworkCore.Migrations;

namespace uwpUI.Core.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Img = table.Column<string>(maxLength: 100, nullable: true),
                    Category = table.Column<string>(maxLength: 25, nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    Weight = table.Column<string>(maxLength: 25, nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Knowledge = table.Column<string>(nullable: true),
                    SellPrice = table.Column<string>(nullable: true),
                    BuyPrice = table.Column<string>(nullable: true),
                    ItemGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_ItemGroups_ItemGroupId",
                        column: x => x.ItemGroupId,
                        principalTable: "ItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Img = table.Column<string>(nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    SkillLevel = table.Column<string>(nullable: true),
                    Exp = table.Column<int>(nullable: true),
                    Item1Id = table.Column<int>(nullable: true),
                    Item2Id = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Items_Item1Id",
                        column: x => x.Item1Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recipes_Items_Item2Id",
                        column: x => x.Item2Id,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RecipeMats",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Amount = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false),
                    IsItem = table.Column<bool>(nullable: false),
                    ItemId = table.Column<int>(nullable: true),
                    ItemGroupId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeMats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeMats_ItemGroups_ItemGroupId",
                        column: x => x.ItemGroupId,
                        principalTable: "ItemGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeMats_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RecipeMats_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemGroupId",
                table: "Items",
                column: "ItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMats_ItemGroupId",
                table: "RecipeMats",
                column: "ItemGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMats_ItemId",
                table: "RecipeMats",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeMats_RecipeId",
                table: "RecipeMats",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Item1Id",
                table: "Recipes",
                column: "Item1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_Item2Id",
                table: "Recipes",
                column: "Item2Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeMats");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemGroups");
        }
    }
}
