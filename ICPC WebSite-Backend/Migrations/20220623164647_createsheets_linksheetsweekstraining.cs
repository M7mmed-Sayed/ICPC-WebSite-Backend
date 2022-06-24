using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class createsheets_linksheetsweekstraining : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_Trainings_TrainingId",
                table: "Weeks");

            migrationBuilder.DropIndex(
                name: "IX_Weeks_TrainingId",
                table: "Weeks");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "Weeks");

            migrationBuilder.AddColumn<int>(
                name: "CommunityId",
                table: "Weeks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Sheets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommunityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sheets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sheets_Communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "Communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeksTrainings",
                columns: table => new
                {
                    WeekId = table.Column<int>(type: "int", nullable: false),
                    TrainingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeksTrainings", x => new { x.TrainingId, x.WeekId });
                    table.ForeignKey(
                        name: "FK_WeeksTrainings_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeksTrainings_Weeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Weeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeksSheets",
                columns: table => new
                {
                    WeekId = table.Column<int>(type: "int", nullable: false),
                    SheetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeksSheets", x => new { x.SheetId, x.WeekId });
                    table.ForeignKey(
                        name: "FK_WeeksSheets_Sheets_SheetId",
                        column: x => x.SheetId,
                        principalTable: "Sheets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeeksSheets_Weeks_WeekId",
                        column: x => x.WeekId,
                        principalTable: "Weeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_Sheets_CommunityId",
                table: "Sheets",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeksSheets_WeekId",
                table: "WeeksSheets",
                column: "WeekId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeksTrainings_WeekId",
                table: "WeeksTrainings",
                column: "WeekId");

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

            migrationBuilder.DropTable(
                name: "WeeksSheets");

            migrationBuilder.DropTable(
                name: "WeeksTrainings");

            migrationBuilder.DropTable(
                name: "Sheets");

            migrationBuilder.DropIndex(
                name: "IX_Weeks_CommunityId",
                table: "Weeks");

            migrationBuilder.DropColumn(
                name: "CommunityId",
                table: "Weeks");

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "Weeks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Weeks_TrainingId",
                table: "Weeks",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_Trainings_TrainingId",
                table: "Weeks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
