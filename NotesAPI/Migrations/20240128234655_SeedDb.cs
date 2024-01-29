using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotesAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "DateCreatedUtc", "DateModifiedUtc", "Tag", "UserId" },
                values: new object[,]
                {
                    { 1L, "test email@test.xyz 123", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "EMAIL", 1L },
                    { 2L, "test +48123123123 abc", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "PHONE", 1L },
                    { 3L, "test 123", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "NONE", 1L },
                    { 4L, "test 2 512312522 abc", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "PHONE", 1L },
                    { 5L, "test 123", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "NONE", 2L },
                    { 6L, "test 123", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "NONE", 2L },
                    { 7L, "test 2 512312522 abc", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "PHONE", 3L },
                    { 8L, "test 2 512312522 abc", new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), null, "PHONE", 3L }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DateCreatedUtc", "Email", "PasswordHash" },
                values: new object[,]
                {
                    { 1L, new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), "user1@test.xyz", "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO" },
                    { 2L, new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), "user2@test.xyz", "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO" },
                    { 3L, new DateTime(2024, 1, 29, 12, 12, 12, 0, DateTimeKind.Utc), "user3@test.xyz", "$2b$10$lPZLjlB/FKie7/evfHnwiO6/vjgm3Jt00AYDOVGgqX3lM4u1tAtoO" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "Notes",
                keyColumn: "Id",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
