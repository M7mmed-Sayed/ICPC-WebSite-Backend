using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class week_community_relationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Weeks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_Communities_CommunityId",
                table: "Weeks",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_Communities_CommunityId",
                table: "Weeks");

            migrationBuilder.DropIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Weeks");
        }
    }
}
