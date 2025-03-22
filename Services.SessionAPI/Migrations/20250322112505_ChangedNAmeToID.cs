using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.SessionAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNAmeToID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerName",
                table: "SessionAssigned",
                newName: "TrainerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrainerID",
                table: "SessionAssigned",
                newName: "TrainerName");
        }
    }
}
