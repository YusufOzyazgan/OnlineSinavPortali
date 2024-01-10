using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSinavPortali.Migrations
{
    /// <inheritdoc />
    public partial class OptionTextABCD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "optionAText",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "optionBText",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "optionCText",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "optionDText",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "optionAText",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "optionBText",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "optionCText",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "optionDText",
                table: "Questions");
        }
    }
}
