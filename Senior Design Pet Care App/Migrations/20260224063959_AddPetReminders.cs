using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SeniorDesignPetCareApp.Migrations
{
    /// <inheritdoc />
    public partial class AddPetReminders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PetId",
                table: "Reminders",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_PetId",
                table: "Reminders",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reminders_Pets_PetId",
                table: "Reminders",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminders_Pets_PetId",
                table: "Reminders");

            migrationBuilder.DropIndex(
                name: "IX_Reminders_PetId",
                table: "Reminders");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Reminders");
        }
    }
}
