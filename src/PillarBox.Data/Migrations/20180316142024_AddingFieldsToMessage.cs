using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace PillarBox.Data.Migrations
{
    public partial class AddingFieldsToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bcc",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cc",
                table: "Messages",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bcc",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Cc",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Messages");
        }
    }
}
