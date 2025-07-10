using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN222_English_Exam.Migrations
{
    /// <inheritdoc />
    public partial class examscore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCorrect",
                table: "Option",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "Score",
                table: "Answer",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCorrect",
                table: "Option");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Answer");
        }
    }
}
