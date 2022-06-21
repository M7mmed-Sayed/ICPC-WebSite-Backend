using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class fix_typing_isuues_AddRelationweek_and_training_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMember_communities_CommunityId",
                table: "CommunityMember");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityRequests_communities_CommunityId",
                table: "CommunityRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_weeks_weekId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Training_communities_CommunityId",
                table: "Training");

            migrationBuilder.DropForeignKey(
                name: "FK_weeks_Training_TrainingId",
                table: "weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_weeks",
                table: "weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_communities",
                table: "communities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Training",
                table: "Training");

            migrationBuilder.DropColumn(
                name: "Training_Id",
                table: "weeks");

            migrationBuilder.DropColumn(
                name: "Community_Id",
                table: "Training");

            migrationBuilder.RenameTable(
                name: "weeks",
                newName: "Weeks");

            migrationBuilder.RenameTable(
                name: "communities",
                newName: "Communities");

            migrationBuilder.RenameTable(
                name: "Training",
                newName: "Trainings");

            migrationBuilder.RenameColumn(
                name: "Updated_at",
                table: "Weeks",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "Weeks",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_weeks_TrainingId",
                table: "Weeks",
                newName: "IX_Weeks_TrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_weeks_Name",
                table: "Weeks",
                newName: "IX_Weeks_Name");

            migrationBuilder.RenameColumn(
                name: "weekId",
                table: "Materials",
                newName: "WeekId");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "Materials",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "Updated_at",
                table: "Materials",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created_at",
                table: "Materials",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_weekId",
                table: "Materials",
                newName: "IX_Materials_WeekId");

            migrationBuilder.RenameColumn(
                name: "Upated_At",
                table: "Trainings",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "Trainings",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "IX_Training_CommunityId",
                table: "Trainings",
                newName: "IX_Trainings_CommunityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weeks",
                table: "Weeks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Communities",
                table: "Communities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trainings",
                table: "Trainings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMember_Communities_CommunityId",
                table: "CommunityMember",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityRequests_Communities_CommunityId",
                table: "CommunityRequests",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_Weeks_WeekId",
                table: "Materials",
                column: "WeekId",
                principalTable: "Weeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Communities_CommunityId",
                table: "Trainings",
                column: "CommunityId",
                principalTable: "Communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Weeks_Trainings_TrainingId",
                table: "Weeks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommunityMember_Communities_CommunityId",
                table: "CommunityMember");

            migrationBuilder.DropForeignKey(
                name: "FK_CommunityRequests_Communities_CommunityId",
                table: "CommunityRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_Weeks_WeekId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Communities_CommunityId",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Weeks_Trainings_TrainingId",
                table: "Weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weeks",
                table: "Weeks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Communities",
                table: "Communities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trainings",
                table: "Trainings");

            migrationBuilder.RenameTable(
                name: "Weeks",
                newName: "weeks");

            migrationBuilder.RenameTable(
                name: "Communities",
                newName: "communities");

            migrationBuilder.RenameTable(
                name: "Trainings",
                newName: "Training");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "weeks",
                newName: "Updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "weeks",
                newName: "Created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Weeks_TrainingId",
                table: "weeks",
                newName: "IX_weeks_TrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_Weeks_Name",
                table: "weeks",
                newName: "IX_weeks_Name");

            migrationBuilder.RenameColumn(
                name: "WeekId",
                table: "Materials",
                newName: "weekId");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Materials",
                newName: "URL");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Materials",
                newName: "Updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Materials",
                newName: "Created_at");

            migrationBuilder.RenameIndex(
                name: "IX_Materials_WeekId",
                table: "Materials",
                newName: "IX_Materials_weekId");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Training",
                newName: "Upated_At");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Training",
                newName: "Created_At");

            migrationBuilder.RenameIndex(
                name: "IX_Trainings_CommunityId",
                table: "Training",
                newName: "IX_Training_CommunityId");

            migrationBuilder.AddColumn<int>(
                name: "Training_Id",
                table: "weeks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Community_Id",
                table: "Training",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_weeks",
                table: "weeks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_communities",
                table: "communities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Training",
                table: "Training",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityMember_communities_CommunityId",
                table: "CommunityMember",
                column: "CommunityId",
                principalTable: "communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommunityRequests_communities_CommunityId",
                table: "CommunityRequests",
                column: "CommunityId",
                principalTable: "communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_weeks_weekId",
                table: "Materials",
                column: "weekId",
                principalTable: "weeks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
