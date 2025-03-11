﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Services.TrainerAPI.Infrastructure;

#nullable disable

namespace Services.TrainerAPI.Migrations
{
    [DbContext(typeof(TrainerDbContext))]
    partial class TrainerDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Services.TrainerAPI.Models.Trainer", b =>
                {
                    b.Property<int>("TrainerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TrainerID"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateJoinedTheClub")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TrainerID");

                    b.ToTable("Trainers");

                    b.HasData(
                        new
                        {
                            TrainerID = 1,
                            Address = "Bulevar Kralja Aleksandra 50, Beograd",
                            DateJoinedTheClub = new DateTime(2015, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOfBirth = new DateTime(1985, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sara Djokic",
                            PhoneNumber = "0641234567"
                        },
                        new
                        {
                            TrainerID = 2,
                            Address = "Njegoševa 15, Novi Sad",
                            DateJoinedTheClub = new DateTime(2017, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOfBirth = new DateTime(1990, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Sandra Kovacevic",
                            PhoneNumber = "0659876543"
                        },
                        new
                        {
                            TrainerID = 3,
                            Address = "Kralja Petra 88, Niš",
                            DateJoinedTheClub = new DateTime(2012, 9, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateOfBirth = new DateTime(1982, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Vladimir Lazarevic",
                            PhoneNumber = "063555888"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
