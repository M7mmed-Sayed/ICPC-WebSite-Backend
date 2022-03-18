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
                    { "178e12a2-96ae-4c2b-b839-c1166fb43427", "b595a9b3-1311-4cb0-9ecb-7afe84a4d5eb", "CommunityLeader", "COMMUNITYLEADER" },
                    { "2cf04b6f-8530-4f2b-b9c9-34b2e032e0f4", "6ea661e4-34e6-4f97-bf1a-7eda6c838ea2", "TrainingManager", "TRAININGMANAGER" },
                    { "5788a0d8-f987-4937-928d-f75471b0426a", "beabc0a6-781c-440f-b29c-31a5b6d5b607", "HeadOfTraining", "HEADOFTRAINING" },
                    { "930c87d5-24b4-4774-8300-4cc7a658de5c", "d061130d-3ca8-4cc9-a563-815a42c3cd1b", "Trainee", "TRAINEE" },
                    { "bbea7e85-803c-4ec1-afc8-97518d2a8815", "11802107-9eed-4535-9860-160e93352493", "Mentor", "MENTOR" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "178e12a2-96ae-4c2b-b839-c1166fb43427");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2cf04b6f-8530-4f2b-b9c9-34b2e032e0f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5788a0d8-f987-4937-928d-f75471b0426a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "930c87d5-24b4-4774-8300-4cc7a658de5c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbea7e85-803c-4ec1-afc8-97518d2a8815");
        }
    }
}
