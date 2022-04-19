using Microsoft.EntityFrameworkCore.Migrations;

namespace Timetable.DbContext.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Colleges_DepartmentId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Departments",
                newName: "CollegeId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_DepartmentId",
                table: "Departments",
                newName: "IX_Departments_CollegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Colleges_CollegeId",
                table: "Departments",
                column: "CollegeId",
                principalTable: "Colleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Colleges_CollegeId",
                table: "Departments");

            migrationBuilder.RenameColumn(
                name: "CollegeId",
                table: "Departments",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Departments_CollegeId",
                table: "Departments",
                newName: "IX_Departments_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Colleges_DepartmentId",
                table: "Departments",
                column: "DepartmentId",
                principalTable: "Colleges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
