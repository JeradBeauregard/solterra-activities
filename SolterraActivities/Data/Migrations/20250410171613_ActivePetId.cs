using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SolterraActivities.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActivePetId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivePetId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePetId",
                table: "Users");
        }
    }
}
