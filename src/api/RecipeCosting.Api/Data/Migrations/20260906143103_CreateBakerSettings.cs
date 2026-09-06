using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RecipeCosting.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateBakerSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "baker_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    monthly_income = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    hours_per_day = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    days_per_week = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    monthly_fixed_costs = table.Column<decimal>(type: "numeric(12,2)", nullable: false),
                    default_markup = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    card_fee = table.Column<decimal>(type: "numeric(6,2)", nullable: false),
                    tax = table.Column<decimal>(type: "numeric(6,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baker_settings", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "baker_settings",
                columns: new[] { "id", "card_fee", "days_per_week", "default_markup", "hours_per_day", "monthly_fixed_costs", "monthly_income", "tax" },
                values: new object[] { 1, 3.5m, 5m, 100m, 6m, 1250m, 4000m, 6m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "baker_settings");
        }
    }
}
