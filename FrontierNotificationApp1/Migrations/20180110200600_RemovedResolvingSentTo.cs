using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FrontierNotificationApp1.Migrations
{
    public partial class RemovedResolvingSentTo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Resolving",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "SendTo",
                table: "Incidents");

            migrationBuilder.DropColumn(
                name: "Resolving",
                table: "ChangeResquests");

            migrationBuilder.DropColumn(
                name: "SendTo",
                table: "ChangeResquests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Resolving",
                table: "Incidents",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SendTo",
                table: "Incidents",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Resolving",
                table: "ChangeResquests",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SendTo",
                table: "ChangeResquests",
                unicode: false,
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
