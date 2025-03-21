using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Services.SessionAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionAssigned",
                columns: table => new
                {
                    SessionAssignedID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionAssigned", x => x.SessionAssignedID);
                });

            migrationBuilder.CreateTable(
                name: "SessionDetails",
                columns: table => new
                {
                    SessionDetailsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionAssignedID = table.Column<int>(type: "int", nullable: false),
                    HorseID = table.Column<int>(type: "int", nullable: false),
                    IsOnPackage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionDetails", x => x.SessionDetailsID);
                    table.ForeignKey(
                        name: "FK_SessionDetails_SessionAssigned_SessionAssignedID",
                        column: x => x.SessionAssignedID,
                        principalTable: "SessionAssigned",
                        principalColumn: "SessionAssignedID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionDetails_SessionAssignedID",
                table: "SessionDetails",
                column: "SessionAssignedID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionDetails");

            migrationBuilder.DropTable(
                name: "SessionAssigned");
        }
    }
}
