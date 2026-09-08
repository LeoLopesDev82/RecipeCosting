using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCosting.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductYield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "yield",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 10,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 11,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 12,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 13,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 14,
                column: "yield",
                value: 1);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 15,
                column: "yield",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "yield",
                table: "products");
        }
    }
}
