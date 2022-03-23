﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class AddRequesterIdinCommunity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RequesterId",
                table: "communities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "communities");
        }
    }
}
