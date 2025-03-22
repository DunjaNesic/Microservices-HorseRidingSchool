using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.TrainerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedTrainerPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TrainerPrice",
                table: "Trainers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 1,
                column: "TrainerPrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 2,
                column: "TrainerPrice",
                value: 0.0);

            migrationBuilder.UpdateData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 3,
                column: "TrainerPrice",
                value: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainerPrice",
                table: "Trainers");
        }
    }
}
