using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.HorseAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedHorsePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "HorsePrice",
                table: "Horses",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 1,
                column: "HorsePrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 2,
                column: "HorsePrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 3,
                column: "HorsePrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 4,
                column: "HorsePrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 5,
                column: "HorsePrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Horses",
                keyColumn: "HorseID",
                keyValue: 6,
                column: "HorsePrice",
                value: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorsePrice",
                table: "Horses");
        }
    }
}
