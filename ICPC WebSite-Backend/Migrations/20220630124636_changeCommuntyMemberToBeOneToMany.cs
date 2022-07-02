using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class changeCommuntyMemberToBeOneToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityRequests",
                table: "CommunityRequests");

            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityRequests",
                table: "CommunityRequests",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommunityId",
                table: "AspNetUsers",
                column: "CommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Communities_CommunityId",
                table: "AspNetUsers",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Communities_CommunityId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommunityRequests",
                table: "CommunityRequests");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommunityId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "AspNetUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommunityRequests",
                table: "CommunityRequests",
                columns: new[] { "MemberId", "CommunityId" });

            migrationBuilder.CreateTable(
                name: "CommunityMember",
                columns: table => new
                {
                    MemberId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMember", x => new { x.MemberId, x.CommunityId, x.Role });
                    table.ForeignKey(
                        name: "FK_CommunityMember_AspNetUsers_MemberId",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommunityMember_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMember_CommunityId",
                table: "CommunityMember",
                column: "CommunityId");
        }
    }
}
