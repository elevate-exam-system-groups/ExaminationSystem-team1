using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExaminationSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSchemaForStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Student_StudentIdEntity",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Student_StudentIdEntity",
                table: "QuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student");

            migrationBuilder.DropIndex(
                name: "IX_OutboxState_BusName_Created",
                table: "OutboxState");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student",
                table: "Student");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admin",
                table: "Admin");

            migrationBuilder.DropColumn(
                name: "BusName",
                table: "OutboxState");

            migrationBuilder.RenameTable(
                name: "Student",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "Admin",
                newName: "Admins");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Students",
                table: "Students",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admins",
                table: "Admins",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admins_AspNetUsers_UserId",
                table: "Admins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Students_StudentIdEntity",
                table: "Enrollments",
                column: "StudentIdEntity",
                principalTable: "Students",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Students_StudentIdEntity",
                table: "QuizAttempt",
                column: "StudentIdEntity",
                principalTable: "Students",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admins_AspNetUsers_UserId",
                table: "Admins");

            migrationBuilder.DropForeignKey(
                name: "FK_Enrollments_Students_StudentIdEntity",
                table: "Enrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_QuizAttempt_Students_StudentIdEntity",
                table: "QuizAttempt");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Students",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Admins",
                table: "Admins");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Student");

            migrationBuilder.RenameTable(
                name: "Admins",
                newName: "Admin");

            migrationBuilder.AddColumn<string>(
                name: "BusName",
                table: "OutboxState",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student",
                table: "Student",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Admin",
                table: "Admin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_BusName_Created",
                table: "OutboxState",
                columns: new[] { "BusName", "Created" });

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_AspNetUsers_UserId",
                table: "Admin",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Enrollments_Student_StudentIdEntity",
                table: "Enrollments",
                column: "StudentIdEntity",
                principalTable: "Student",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuizAttempt_Student_StudentIdEntity",
                table: "QuizAttempt",
                column: "StudentIdEntity",
                principalTable: "Student",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_AspNetUsers_UserId",
                table: "Student",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
