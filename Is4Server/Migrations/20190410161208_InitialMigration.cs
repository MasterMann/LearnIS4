using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Is4Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    HashedPassword = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Birthday = table.Column<double>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    JoinedTime = table.Column<double>(nullable: false),
                    LastModifiedTime = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
