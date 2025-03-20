using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.HorseAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Horses",
                columns: table => new
                {
                    HorseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Horses", x => x.HorseID);
                });

            migrationBuilder.InsertData(
                table: "Horses",
                columns: new[] { "HorseID", "Breed", "DateOfBirth", "Name" },
                values: new object[,]
                {
                    { 1, "Thoroughbred", new DateTime(2016, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Waban" },
                    { 2, "Arabian", new DateTime(2016, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Silver Static" },
                    { 3, "Quarter Horse", new DateTime(2014, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beba" },
                    { 4, "Paint Horse", new DateTime(2017, 2, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sena" },
                    { 5, "Mustang", new DateTime(2018, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Crystal Night" },
                    { 6, "Pony", new DateTime(2015, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Princess" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Horses");
        }
    }
}
