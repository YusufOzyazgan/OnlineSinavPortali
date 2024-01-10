using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineSinavPortali.Migrations
{
    /// <inheritdoc />
    public partial class DepartmentExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UniversityDepartment",
                table: "Exams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UniversityDepartment",
                table: "Exams");
        }
    }
}
