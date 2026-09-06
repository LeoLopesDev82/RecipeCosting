using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeCosting.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    package_size = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    package_unit = table.Column<int>(type: "integer", nullable: false),
                    package_price = table.Column<decimal>(type: "numeric(12,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "ingredients",
                columns: new[] { "id", "name", "package_price", "package_size", "package_unit" },
                values: new object[,]
                {
                    { 1, "Condensed milk", 7.49m, 395m, 1 },
                    { 2, "Table cream", 4.29m, 200m, 1 },
                    { 3, "Whole milk", 5.49m, 1000m, 2 },
                    { 4, "Cream cheese", 9.9m, 150m, 1 },
                    { 5, "Whipping cream", 28.9m, 1000m, 2 },
                    { 6, "Unsalted butter", 12.9m, 200m, 1 },
                    { 7, "Baking margarine", 9.9m, 500m, 1 },
                    { 8, "Vegetable oil", 8.49m, 900m, 2 },
                    { 9, "All-purpose flour", 5.79m, 1000m, 1 },
                    { 10, "Cornstarch", 6.49m, 500m, 1 },
                    { 11, "Cornmeal", 4.19m, 500m, 1 },
                    { 12, "Granulated sugar", 4.89m, 1000m, 1 },
                    { 13, "Icing sugar", 5.29m, 500m, 1 },
                    { 14, "Brown sugar", 8.9m, 500m, 1 },
                    { 15, "Glucose syrup", 12.5m, 500m, 1 },
                    { 16, "Honey", 18.9m, 250m, 1 },
                    { 17, "Cocoa powder", 14.9m, 200m, 1 },
                    { 18, "Semisweet chocolate", 42.9m, 1000m, 1 },
                    { 19, "Milk chocolate", 44.9m, 1000m, 1 },
                    { 20, "White chocolate", 46.5m, 1000m, 1 },
                    { 21, "Roasted peanuts", 14.9m, 500m, 1 },
                    { 22, "Walnut halves", 19.9m, 100m, 1 },
                    { 23, "Grated coconut", 6.9m, 100m, 1 },
                    { 24, "Strawberries", 12.9m, 300m, 1 },
                    { 25, "Vanilla extract", 18.9m, 100m, 2 },
                    { 26, "Red food colouring", 7.9m, 30m, 2 },
                    { 27, "Lemon", 9.6m, 12m, 3 },
                    { 28, "Chocolate sprinkles", 13.9m, 500m, 1 },
                    { 29, "Confetti sprinkles", 8.4m, 100m, 1 },
                    { 30, "Eggs", 24.9m, 30m, 3 },
                    { 31, "Baking powder", 4.19m, 100m, 1 },
                    { 32, "Unflavoured gelatin", 5.9m, 24m, 1 },
                    { 33, "Fine salt", 3.29m, 1000m, 1 },
                    { 34, "Cupcake liners", 9.9m, 100m, 3 },
                    { 35, "Cake box 20 cm", 24.0m, 10m, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ingredients");
        }
    }
}
