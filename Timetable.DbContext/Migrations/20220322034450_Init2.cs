using Microsoft.EntityFrameworkCore.Migrations;

namespace Timetable.DbContext.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePIcks_Courses_CourseId",
                table: "CoursePIcks");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePIcks_Courses_CourseId",
                table: "CoursePIcks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePIcks_Courses_CourseId",
                table: "CoursePIcks");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePIcks_Courses_CourseId",
                table: "CoursePIcks",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
