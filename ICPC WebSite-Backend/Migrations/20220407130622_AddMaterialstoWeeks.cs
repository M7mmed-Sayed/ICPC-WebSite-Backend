using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class AddMaterialstoWeeks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "weekId",
                table: "matirials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_matirials_weekId",
                table: "matirials",
                column: "weekId");

            migrationBuilder.AddForeignKey(
                name: "FK_matirials_weeks_weekId",
                table: "matirials",
                column: "weekId",
                principalTable: "weeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_matirials_weeks_weekId",
                table: "matirials");

            migrationBuilder.DropIndex(
                name: "IX_matirials_weekId",
                table: "matirials");

            migrationBuilder.DropColumn(
                name: "weekId",
                table: "matirials");
        }
    }
}
