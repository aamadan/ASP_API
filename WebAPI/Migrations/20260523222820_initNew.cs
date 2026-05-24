using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class initNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7deee592-cd40-447e-a8dc-2b3bae55ee2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b427d6b3-902c-4741-9100-8ffcd5ff2895");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ba1139c2-2a96-4742-a4ff-1c8581cc4d5f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2e0b17ca-6392-4826-9ed9-fd0f88c4cf40", "e73b24b7-2afc-48db-a7e3-a2d34ec774a9", "Staff", "STAFF" },
                    { "438b8df5-13d0-4875-911a-a03bf03a9bdd", "36faf993-b709-4182-9a08-758a138e7a4f", "User", "USER" },
                    { "f54606ee-4687-4d01-9e36-d2b6f711021e", "7348320a-5b91-4195-83ea-d1981f3b77e5", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e0b17ca-6392-4826-9ed9-fd0f88c4cf40");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "438b8df5-13d0-4875-911a-a03bf03a9bdd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f54606ee-4687-4d01-9e36-d2b6f711021e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7deee592-cd40-447e-a8dc-2b3bae55ee2c", "1bc508cc-d913-4ba9-a902-5127f7857966", "Admin", "ADMIN" },
                    { "b427d6b3-902c-4741-9100-8ffcd5ff2895", "cdcdfe9f-9e92-4241-b468-b97de81507e5", "User", "USER" },
                    { "ba1139c2-2a96-4742-a4ff-1c8581cc4d5f", "464937d0-04a6-4643-af59-c085900a9aa8", "Staff", "STAFF" }
                });
        }
    }
}
