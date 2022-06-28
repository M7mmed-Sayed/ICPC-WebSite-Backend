using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class remove_Week_Community_Link : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sheets_Communities_CommunityId",
                table: "Sheets");

            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_Communities_CommunityId",
                table: "Weeks");

            migrationBuilder.DropIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks");

            migrationBuilder.DropIndex(
                name: "IX_Sheets_CommunityId",
                table: "Sheets");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Weeks");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Sheets");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Weeks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Sheets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_CommunityId",
                table: "Sheets",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sheets_Communities_CommunityId",
                table: "Sheets",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_Communities_CommunityId",
                table: "Weeks",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }
    }
}
