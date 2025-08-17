using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace universityManagementSys.Migrations
{
    /// <inheritdoc />
    public partial class YallaYabnAlcomputer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_grades", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "instructors",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_instructors", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "semesters",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_semesters", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "students",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNum = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_students", x => x.ID);
                    table.ForeignKey(
                        name: "FK_students_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditHours = table.Column<int>(type: "int", nullable: false),
                    InstructorID = table.Column<int>(type: "int", nullable: false),
                    SemesterID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_courses_instructors_InstructorID",
                        column: x => x.InstructorID,
                        principalTable: "instructors",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_courses_semesters_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "semesters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "departmentCourses",
                columns: table => new
                {
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmentCourses", x => new { x.DepartmentID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_departmentCourses_courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_departmentCourses_departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "departments",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    CourseID = table.Column<int>(type: "int", nullable: false),
                    GradeID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollments", x => new { x.StudentID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_enrollments_courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollments_grades_GradeID",
                        column: x => x.GradeID,
                        principalTable: "grades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_enrollments_students_StudentID",
                        column: x => x.StudentID,
                        principalTable: "students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_courses_InstructorID",
                table: "courses",
                column: "InstructorID");

            migrationBuilder.CreateIndex(
                name: "IX_courses_SemesterID",
                table: "courses",
                column: "SemesterID");

            migrationBuilder.CreateIndex(
                name: "IX_departmentCourses_CourseID",
                table: "departmentCourses",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_CourseID",
                table: "enrollments",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_GradeID",
                table: "enrollments",
                column: "GradeID");

            migrationBuilder.CreateIndex(
                name: "IX_students_DepartmentID",
                table: "students",
                column: "DepartmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "departmentCourses");

            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "students");

            migrationBuilder.DropTable(
                name: "instructors");

            migrationBuilder.DropTable(
                name: "semesters");

            migrationBuilder.DropTable(
                name: "departments");
        }
    }
}
