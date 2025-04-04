using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolterraActivities.Data.Migrations
{
    /// <inheritdoc />
    public partial class mood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "moood",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Pets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "strength",
                table: "Pets",
                newName: "Strength");

            migrationBuilder.RenameColumn(
                name: "level",
                table: "Pets",
                newName: "Level");

            migrationBuilder.RenameColumn(
                name: "intelligence",
                table: "Pets",
                newName: "Intelligence");

            migrationBuilder.RenameColumn(
                name: "hunger",
                table: "Pets",
                newName: "Hunger");

            migrationBuilder.RenameColumn(
                name: "health",
                table: "Pets",
                newName: "Health");

            migrationBuilder.RenameColumn(
                name: "defence",
                table: "Pets",
                newName: "Defence");

            migrationBuilder.RenameColumn(
                name: "agility",
                table: "Pets",
                newName: "Agility");

            migrationBuilder.RenameColumn(
                name: "species_id",
                table: "Pets",
                newName: "SpeciesId");

            migrationBuilder.AddColumn<string>(
                name: "Mood",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mood",
                table: "Pets");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Pets",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Strength",
                table: "Pets",
                newName: "strength");

            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Pets",
                newName: "level");

            migrationBuilder.RenameColumn(
                name: "Intelligence",
                table: "Pets",
                newName: "intelligence");

            migrationBuilder.RenameColumn(
                name: "Hunger",
                table: "Pets",
                newName: "hunger");

            migrationBuilder.RenameColumn(
                name: "Health",
                table: "Pets",
                newName: "health");

            migrationBuilder.RenameColumn(
                name: "Defence",
                table: "Pets",
                newName: "defence");

            migrationBuilder.RenameColumn(
                name: "Agility",
                table: "Pets",
                newName: "agility");

            migrationBuilder.RenameColumn(
                name: "SpeciesId",
                table: "Pets",
                newName: "species_id");

            migrationBuilder.AddColumn<int>(
                name: "moood",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
