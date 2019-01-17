using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace FrontierNotificationApp1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Department = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    EmailAddress = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    LastName = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    UserId = table.Column<string>(unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeResquests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Impact = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Resolving = table.Column<string>(nullable: true),
                    SendTo = table.Column<string>(nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Status = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    Subject = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeResquests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CurrentDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Impact = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    InitialDateTime = table.Column<DateTime>(type: "smalldatetime", nullable: false),
                    Priority = table.Column<string>(unicode: false, maxLength: 10, nullable: false),
                    Resolving = table.Column<string>(unicode: false, maxLength: 255, nullable: true),
                    SDPTicket = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    SendTo = table.Column<string>(nullable: true),
                    Status = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    Subject = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: false),
                    VendorTicket = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Administrators");

            migrationBuilder.DropTable(
                name: "ChangeResquests");

            migrationBuilder.DropTable(
                name: "Incidents");
        }
    }
}
