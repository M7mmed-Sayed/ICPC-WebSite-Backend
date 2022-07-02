using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class remove_Is__Approved_fromCommunity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Communities");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Communities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
