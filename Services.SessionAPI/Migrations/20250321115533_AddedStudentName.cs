using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.SessionAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedStudentName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "SessionDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "SessionDetails");
        }
    }
}
