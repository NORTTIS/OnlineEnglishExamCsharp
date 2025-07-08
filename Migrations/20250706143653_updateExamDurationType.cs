using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PRN222_English_Exam.Migrations
{
    /// <inheritdoc />
    public partial class updateExamDurationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "Duration",
                 table: "Exam");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Exam",
                type: "int",
                nullable: false,
                defaultValue: 0); // hoặc giá trị mặc định khác

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Duration",
                table: "Exam",
                type: "time",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
