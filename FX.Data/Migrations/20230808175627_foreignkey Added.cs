using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FX.Data.Migrations
{
    /// <inheritdoc />
    public partial class foreignkeyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_CourseUploads_CourseUploadCourseId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseUploadCourseId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CourseUploadCourseId",
                table: "Lessons");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseId",
                table: "Lessons",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_CourseUploads_CourseId",
                table: "Lessons",
                column: "CourseId",
                principalTable: "CourseUploads",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lessons_CourseUploads_CourseId",
                table: "Lessons");

            migrationBuilder.DropIndex(
                name: "IX_Lessons_CourseId",
                table: "Lessons");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Lessons");

            migrationBuilder.AddColumn<Guid>(
                name: "CourseUploadCourseId",
                table: "Lessons",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_CourseUploadCourseId",
                table: "Lessons",
                column: "CourseUploadCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lessons_CourseUploads_CourseUploadCourseId",
                table: "Lessons",
                column: "CourseUploadCourseId",
                principalTable: "CourseUploads",
                principalColumn: "CourseId");
        }
    }
}
