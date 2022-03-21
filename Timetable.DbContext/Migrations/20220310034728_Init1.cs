using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Timetable.DbContext.Migrations
{
    public partial class Init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Name_CN = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Name_EN = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SEOKeywords_CN = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SEOKeywords_EN = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Description_CN = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Description_EN = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    VisibleInArticle = table.Column<bool>(type: "boolean", nullable: false),
                    HasSpecialUrl = table.Column<bool>(type: "boolean", nullable: false),
                    SpecialUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SortNum = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Channels_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Colleges",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colleges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    Url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Headshot = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SN = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionGroups_PermissionGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Remark = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTerm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Year = table.Column<int>(type: "integer", nullable: false),
                    Term = table.Column<int>(type: "integer", nullable: false),
                    CustomStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CustomEnd = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FixedStart = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTerm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    IdentityCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SchoolOpenId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SinglePointHash = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    RealName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.IdentityCode);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ContactName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ContactPhone = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Remark = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_Colleges_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermissionLines",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Key = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    GroupId = table.Column<long>(type: "bigint", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Url = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    SN = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermissionLines_PermissionGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "PermissionGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeClasses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CollegeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Grade = table.Column<int>(type: "integer", nullable: false),
                    FullName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    AdministrativeClassState = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeClasses_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdministrativeClasses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    LoginName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    Headshot = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    RealName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    Mobile = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PermissionEndTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatorId = table.Column<long>(type: "bigint", nullable: true),
                    SinglePointHsah = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    SubjectCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    CollegeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    SubjectType = table.Column<int>(type: "integer", nullable: false),
                    SubjectProperty = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.SubjectCode);
                    table.ForeignKey(
                        name: "FK_Subjects_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subjects_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionLineId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermissions_PermissionLines_PermissionLineId",
                        column: x => x.PermissionLineId,
                        principalTable: "PermissionLines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdministrativeClassBackups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    BackupList = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    AdminstractiveClassId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdministrativeClassBackups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdministrativeClassBackups_AdministrativeClasses_Adminstrac~",
                        column: x => x.AdminstractiveClassId,
                        principalTable: "AdministrativeClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    IdentityCode = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    SchoolOpenId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    Password = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SinglePointHash = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    RealName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Mobile = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    CollegeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    AdministrativeClassId = table.Column<long>(type: "bigint", nullable: false),
                    AdmissionYear = table.Column<long>(type: "bigint", nullable: false),
                    StudentState = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.IdentityCode);
                    table.ForeignKey(
                        name: "FK_Students_AdministrativeClasses_AdministrativeClassId",
                        column: x => x.AdministrativeClassId,
                        principalTable: "AdministrativeClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Title_CN = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Title_EN = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Author = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SEOKeywords_CN = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    SEOKeywords_EN = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Description_CN = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Description_EN = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Enable = table.Column<bool>(type: "boolean", nullable: false),
                    SortNum = table.Column<int>(type: "integer", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    ViewCount = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ChannelId = table.Column<long>(type: "bigint", nullable: false),
                    Content_CN = table.Column<string>(type: "character varying(1048576)", maxLength: 1048576, nullable: true),
                    Content_EN = table.Column<string>(type: "character varying(1048576)", maxLength: 1048576, nullable: true),
                    Src = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    PublishData = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Articles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeLoginLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IP = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Success = table.Column<bool>(type: "boolean", nullable: false),
                    LoginTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeLoginLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeLoginLogs_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    SubjectCode = table.Column<string>(type: "character varying(32)", nullable: false),
                    CollegeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    CourseName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    TeacherIdentityCode = table.Column<string>(type: "character varying(32)", nullable: false),
                    AdministrativeClassId = table.Column<long>(type: "bigint", nullable: true),
                    SchoolTermId = table.Column<long>(type: "bigint", nullable: false),
                    TimebookJson = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    AdminstractiveClassBackupId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_AdministrativeClassBackups_AdminstractiveClassBacku~",
                        column: x => x.AdminstractiveClassBackupId,
                        principalTable: "AdministrativeClassBackups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_AdministrativeClasses_AdministrativeClassId",
                        column: x => x.AdministrativeClassId,
                        principalTable: "AdministrativeClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Colleges_CollegeId",
                        column: x => x.CollegeId,
                        principalTable: "Colleges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_SchoolTerm_SchoolTermId",
                        column: x => x.SchoolTermId,
                        principalTable: "SchoolTerm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Subjects_SubjectCode",
                        column: x => x.SubjectCode,
                        principalTable: "Subjects",
                        principalColumn: "SubjectCode",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers_TeacherIdentityCode",
                        column: x => x.TeacherIdentityCode,
                        principalTable: "Teachers",
                        principalColumn: "IdentityCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoursePIcks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CourseId = table.Column<long>(type: "bigint", nullable: false),
                    StudentIdentityCode = table.Column<string>(type: "character varying(32)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursePIcks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoursePIcks_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoursePIcks_Students_StudentIdentityCode",
                        column: x => x.StudentIdentityCode,
                        principalTable: "Students",
                        principalColumn: "IdentityCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeClassBackups_AdminstractiveClassId",
                table: "AdministrativeClassBackups",
                column: "AdminstractiveClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeClasses_CollegeId",
                table: "AdministrativeClasses",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_AdministrativeClasses_DepartmentId",
                table: "AdministrativeClasses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ChannelId",
                table: "Articles",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_EmployeeId",
                table: "Articles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_Key",
                table: "Channels",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channels_ParentId",
                table: "Channels",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePIcks_CourseId",
                table: "CoursePIcks",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePIcks_StudentIdentityCode",
                table: "CoursePIcks",
                column: "StudentIdentityCode");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AdministrativeClassId",
                table: "Courses",
                column: "AdministrativeClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_AdminstractiveClassBackupId",
                table: "Courses",
                column: "AdminstractiveClassBackupId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CollegeId",
                table: "Courses",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_DepartmentId",
                table: "Courses",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SchoolTermId",
                table: "Courses",
                column: "SchoolTermId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_SubjectCode",
                table: "Courses",
                column: "SubjectCode");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherIdentityCode",
                table: "Courses",
                column: "TeacherIdentityCode");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentId",
                table: "Departments",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeLoginLogs_EmployeeId",
                table: "EmployeeLoginLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_EmployeeId",
                table: "EmployeeRoles",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_RoleId",
                table: "EmployeeRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatorId",
                table: "Employees",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_LoginName",
                table: "Employees",
                column: "LoginName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_Key",
                table: "PermissionGroups",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermissionGroups_ParentId",
                table: "PermissionGroups",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLines_GroupId",
                table: "PermissionLines",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionLines_Key",
                table: "PermissionLines",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionLineId",
                table: "RolePermissions",
                column: "PermissionLineId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_AdministrativeClassId",
                table: "Students",
                column: "AdministrativeClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CollegeId",
                table: "Students",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_DepartmentId",
                table: "Students",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolOpenId",
                table: "Students",
                column: "SchoolOpenId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_CollegeId",
                table: "Subjects",
                column: "CollegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DepartmentId",
                table: "Subjects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolOpenId",
                table: "Teachers",
                column: "SchoolOpenId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "CoursePIcks");

            migrationBuilder.DropTable(
                name: "EmployeeLoginLogs");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PermissionLines");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "AdministrativeClassBackups");

            migrationBuilder.DropTable(
                name: "SchoolTerm");

            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "PermissionGroups");

            migrationBuilder.DropTable(
                name: "AdministrativeClasses");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Colleges");
        }
    }
}
