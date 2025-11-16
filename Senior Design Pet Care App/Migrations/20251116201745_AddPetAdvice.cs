using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorDesignPetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPetAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Advice",
                table: "Pets",
                type: "TEXT",
                maxLength: 4000,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advice",
                table: "Pets");
        }
    }
}
