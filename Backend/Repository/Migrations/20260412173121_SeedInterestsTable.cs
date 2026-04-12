using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedInterestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("194214ea-06ad-4094-8211-da560c056cd6"), "Travelling" },
                    { new Guid("35322244-d9be-4a99-9e53-3faa1a70d288"), "Sports" },
                    { new Guid("6cf4a9a0-acd5-46d8-8b2a-bf5985fb5a54"), "Coding" },
                    { new Guid("a0cd2cd1-aac4-4a6a-b307-90a85f454d80"), "Cooking" },
                    { new Guid("d284052f-b675-4efb-83b7-4674d571a988"), "Football" },
                    { new Guid("ee2f1f22-d27f-4bfd-a90a-403eee64f911"), "Chess" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("194214ea-06ad-4094-8211-da560c056cd6"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("35322244-d9be-4a99-9e53-3faa1a70d288"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("6cf4a9a0-acd5-46d8-8b2a-bf5985fb5a54"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("a0cd2cd1-aac4-4a6a-b307-90a85f454d80"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("d284052f-b675-4efb-83b7-4674d571a988"));

            migrationBuilder.DeleteData(
                table: "Interests",
                keyColumn: "Id",
                keyValue: new Guid("ee2f1f22-d27f-4bfd-a90a-403eee64f911"));

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");
        }
    }
}
