using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FrontierNotificationApp1.Migrations
{
    public partial class AddedTeamsEngaged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamsEngaged",
                table: "Incidents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamsEngaged",
                table: "ChangeResquests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamsEngaged",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "TeamsEngaged",
                table: "ChangeResquests");
        }
    }
}
