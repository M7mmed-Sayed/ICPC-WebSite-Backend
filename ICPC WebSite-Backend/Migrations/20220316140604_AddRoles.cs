using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICPC_WebSite_Backend.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1523d97f-5c2d-40b8-ba9e-60859b645825", "5e97147e-bfb5-4bdf-8344-e2560f2b55ff", "Mentor", "MENTOR" },
                    { "5a3e5711-88dd-43a5-ad35-ab980d840f0d", "8de8baa0-33d7-4f19-92ef-1823755da1f3", "CommunityLeader", "COMMUNITYLEADER" },
                    { "7693d26b-0956-47c2-8297-11a2cb86f8b6", "809dfd03-38d9-4d3d-acec-603a4e491f19", "HeadOfTraining", "HEADOFTRAINING" },
                    { "7a2593b1-3483-4994-96ee-e824e8867e11", "1609dafe-0476-4a0e-9587-f4f8b4c9e8be", "TrainingManager", "TRAININGMANAGER" },
                    { "a77b3f45-77f1-41f7-a67c-cadf8bb81049", "1e26b0b5-7e6b-4979-90a8-8b275a9ca586", "Trainee", "TRAINEE" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1523d97f-5c2d-40b8-ba9e-60859b645825");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a3e5711-88dd-43a5-ad35-ab980d840f0d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7693d26b-0956-47c2-8297-11a2cb86f8b6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7a2593b1-3483-4994-96ee-e824e8867e11");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a77b3f45-77f1-41f7-a67c-cadf8bb81049");
        }
    }
}
