using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeCosting.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    prep_minutes = table.Column<decimal>(type: "numeric(8,2)", nullable: false),
                    markup = table.Column<decimal>(type: "numeric(6,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    ingredient_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<decimal>(type: "numeric(12,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_lines", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_lines_ingredients_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredients",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_lines_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "markup", "name", "prep_minutes" },
                values: new object[,]
                {
                    { 1, null, "Brigadeiro", 2m },
                    { 2, null, "Beijinho", 2m },
                    { 3, null, "Chocolate truffle", 3m },
                    { 4, null, "Chocolate bonbon", 4m },
                    { 5, null, "Brownie", 4m },
                    { 6, null, "Cupcake", 5m },
                    { 7, null, "Chocolate cake slice", 6m },
                    { 8, null, "Cheesecake slice", 8m },
                    { 9, null, "Banoffee jar", 10m },
                    { 10, 90m, "Strawberry tart", 12m },
                    { 11, null, "Coconut candy box", 25m },
                    { 12, 80m, "Carrot cake with fudge", 45m },
                    { 13, 90m, "Lemon pie", 60m },
                    { 14, 120m, "Red velvet cake 20 cm", 90m },
                    { 15, 150m, "Wedding cake tier", 180m }
                });

            migrationBuilder.InsertData(
                table: "product_lines",
                columns: new[] { "id", "ingredient_id", "product_id", "quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 18m },
                    { 2, 17, 1, 2m },
                    { 3, 6, 1, 1m },
                    { 4, 28, 1, 4m },
                    { 5, 1, 2, 18m },
                    { 6, 23, 2, 6m },
                    { 7, 6, 2, 1m },
                    { 8, 18, 3, 12m },
                    { 9, 5, 3, 6m },
                    { 10, 6, 3, 1m },
                    { 11, 19, 4, 15m },
                    { 12, 1, 4, 8m },
                    { 13, 6, 4, 1m },
                    { 14, 18, 5, 15m },
                    { 15, 6, 5, 12m },
                    { 16, 12, 5, 20m },
                    { 17, 9, 5, 12m },
                    { 18, 17, 5, 5m },
                    { 19, 30, 5, 0.3m },
                    { 20, 9, 6, 30m },
                    { 21, 12, 6, 25m },
                    { 22, 6, 6, 15m },
                    { 23, 30, 6, 0.5m },
                    { 24, 3, 6, 20m },
                    { 25, 31, 6, 2m },
                    { 26, 34, 6, 1m },
                    { 27, 29, 6, 3m },
                    { 28, 9, 7, 25m },
                    { 29, 12, 7, 30m },
                    { 30, 17, 7, 8m },
                    { 31, 30, 7, 0.5m },
                    { 32, 8, 7, 15m },
                    { 33, 3, 7, 25m },
                    { 34, 31, 7, 2m },
                    { 35, 4, 8, 60m },
                    { 36, 12, 8, 20m },
                    { 37, 30, 8, 0.5m },
                    { 38, 5, 8, 25m },
                    { 39, 24, 8, 20m },
                    { 40, 1, 9, 60m },
                    { 41, 5, 9, 40m },
                    { 42, 6, 9, 10m },
                    { 43, 12, 9, 15m },
                    { 44, 9, 10, 40m },
                    { 45, 6, 10, 25m },
                    { 46, 12, 10, 20m },
                    { 47, 30, 10, 0.5m },
                    { 48, 24, 10, 60m },
                    { 49, 5, 10, 30m },
                    { 50, 1, 11, 200m },
                    { 51, 23, 11, 80m },
                    { 52, 12, 11, 40m },
                    { 53, 6, 11, 10m },
                    { 54, 9, 12, 250m },
                    { 55, 12, 12, 300m },
                    { 56, 30, 12, 3m },
                    { 57, 8, 12, 120m },
                    { 58, 17, 12, 40m },
                    { 59, 1, 12, 395m },
                    { 60, 6, 12, 30m },
                    { 61, 35, 12, 1m },
                    { 62, 1, 13, 790m },
                    { 63, 27, 13, 4m },
                    { 64, 30, 13, 3m },
                    { 65, 12, 13, 150m },
                    { 66, 6, 13, 60m },
                    { 67, 9, 13, 200m },
                    { 68, 35, 13, 1m },
                    { 69, 9, 14, 300m },
                    { 70, 12, 14, 350m },
                    { 71, 4, 14, 300m },
                    { 72, 6, 14, 150m },
                    { 73, 30, 14, 4m },
                    { 74, 26, 14, 15m },
                    { 75, 17, 14, 20m },
                    { 76, 3, 14, 200m },
                    { 77, 35, 14, 1m },
                    { 78, 9, 15, 600m },
                    { 79, 12, 15, 700m },
                    { 80, 6, 15, 400m },
                    { 81, 30, 15, 8m },
                    { 82, 5, 15, 500m },
                    { 83, 20, 15, 400m },
                    { 84, 24, 15, 300m },
                    { 85, 35, 15, 1m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_lines_ingredient_id",
                table: "product_lines",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_lines_product_id",
                table: "product_lines",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_lines");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
