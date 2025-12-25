using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Groups",
                newName: "TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                newName: "IX_Groups_TeacherId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Groups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_BranchId",
                table: "Groups",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Branches_BranchId",
                table: "Groups",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_TeacherId",
                table: "Groups",
                column: "TeacherId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Branches_BranchId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_TeacherId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_BranchId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "Groups",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Groups_TeacherId",
                table: "Groups",
                newName: "IX_Groups_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
