using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class AddTrainigDataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Training_communities_CommunityId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_weeks_Training_TrainingId",
                table: "weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Training",
                table: "Training");

            migrationBuilder.RenameTable(
                name: "Training",
                newName: "trainings");

            migrationBuilder.RenameIndex(
                name: "IX_Training_CommunityId",
                table: "trainings",
                newName: "IX_trainings_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_trainings",
                table: "trainings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_trainings_communities_CommunityId",
                table: "trainings",
                column: "CommunityId",
                principalTable: "communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weeks_trainings_TrainingId",
                table: "weeks",
                column: "TrainingId",
                principalTable: "trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_trainings_communities_CommunityId",
                table: "trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_weeks_trainings_TrainingId",
                table: "weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_trainings",
                table: "trainings");

            migrationBuilder.RenameTable(
                name: "trainings",
                newName: "Training");

            migrationBuilder.RenameIndex(
                name: "IX_trainings_CommunityId",
                table: "Training",
                newName: "IX_Training_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Training",
                table: "Training",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Training_communities_CommunityId",
                table: "Training",
                column: "CommunityId",
                principalTable: "communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_weeks_Training_TrainingId",
                table: "weeks",
                column: "TrainingId",
                principalTable: "Training",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
