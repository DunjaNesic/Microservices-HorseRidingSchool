using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Services.TrainerAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedTrainers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "TrainerID", "Address", "DateJoinedTheClub", "DateOfBirth", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Bulevar Kralja Aleksandra 50, Beograd", new DateTime(2015, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sara Djokic", "0641234567" },
                    { 2, "Njegoševa 15, Novi Sad", new DateTime(2017, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1990, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sandra Kovacevic", "0659876543" },
                    { 3, "Kralja Petra 88, Niš", new DateTime(2012, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1982, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vladimir Lazarevic", "063555888" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "TrainerID",
                keyValue: 3);
        }
    }
}
