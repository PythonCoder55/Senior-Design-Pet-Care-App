using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorDesignPetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddReoccurrances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SeriesId",
                table: "Reminders",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Reminders");
        }
    }
}
