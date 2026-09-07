using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeCosting.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "products",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "ingredients",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "baker_settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "baker_settings",
                keyColumn: "id",
                keyValue: 1,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 1,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 2,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 3,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 4,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 5,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 6,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 7,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 8,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 9,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 10,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 11,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 12,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 13,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 14,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 15,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 16,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 17,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 18,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 19,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 20,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 21,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 22,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 23,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 24,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 25,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 26,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 27,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 28,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 29,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 30,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 31,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 32,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 33,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 34,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "ingredients",
                keyColumn: "id",
                keyValue: 35,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 10,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 11,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 12,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 13,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 14,
                column: "version",
                value: 0);

            migrationBuilder.UpdateData(
                table: "products",
                keyColumn: "id",
                keyValue: 15,
                column: "version",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "version",
                table: "products");

            migrationBuilder.DropColumn(
                name: "version",
                table: "ingredients");

            migrationBuilder.DropColumn(
                name: "version",
                table: "baker_settings");
        }
    }
}
