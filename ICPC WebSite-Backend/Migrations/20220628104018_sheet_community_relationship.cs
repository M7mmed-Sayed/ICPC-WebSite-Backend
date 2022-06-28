using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class sheet_community_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Sheets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_CommunityId",
                table: "Sheets",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sheets_Communities_CommunityId",
                table: "Sheets",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sheets_Communities_CommunityId",
                table: "Sheets");

            migrationBuilder.DropIndex(
                name: "IX_Sheets_CommunityId",
                table: "Sheets");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Sheets");
        }
    }
}
