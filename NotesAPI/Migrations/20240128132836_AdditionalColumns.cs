using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotesAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdditionalColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Notes",
                newName: "DateCreatedUtc");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModifiedUtc",
                table: "Notes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateModifiedUtc",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                table: "Notes",
                newName: "DateCreated");
        }
    }
}
